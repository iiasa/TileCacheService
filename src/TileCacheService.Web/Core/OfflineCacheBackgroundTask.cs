// <copyright file="OfflineCacheBackgroundTask.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using AutoMapper;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;
	using TileCacheService.Data.Entities;
	using TileCacheService.Data.Repositories;
	using TileCacheService.Processing;
	using TileCacheService.Processing.Models;
	using TileCacheService.Shared;
	using TileCacheService.Shared.Helpers;
	using TileCacheService.Shared.Services;
	using TileCacheService.Web.Models;

	public class OfflineCacheBackgroundTask : BackgroundService
	{
		private static object lockObject = new object();

		public OfflineCacheBackgroundTask(ILogger<OfflineCacheBackgroundTask> logger, IServiceScopeFactory scopeFactory,
			Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IOptions<ServiceOptions> serviceOptions)
		{
			Logger = logger;
			ScopeFactory = scopeFactory;
			HostingEnvironment = hostingEnvironment;
			ServiceOptions = serviceOptions.Value;
		}

		public static int TilesCount { get; set; }

		public static int TilesTotal { get; set; }

		public ServiceOptions ServiceOptions { get; }

		public Microsoft.AspNetCore.Hosting.IHostingEnvironment HostingEnvironment { get; set; }

		public ILogger<BackgroundService> Logger { get; set; }

		public IMapper Mapper { get; set; }

		public IServiceScopeFactory ScopeFactory { get; set; }

		public TileCacheRepository TileCacheRepository { get; set; }

		public TileSourceRepository TileSourceRepository { get; set; }

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			Logger.LogInformation("Running OfflineCacheBackgroundTask.");

			using (IServiceScope scope = ScopeFactory.CreateScope())
			{
				TileCacheRepository = scope.ServiceProvider.GetRequiredService<TileCacheRepository>();
				TileSourceRepository = scope.ServiceProvider.GetRequiredService<TileSourceRepository>();
				Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

				do
				{
					TileCacheViewModel unfinishedTileCache =
						Mapper.Map<TileCacheViewModel>(await TileCacheRepository.GetFirstUnfinishedTileCache());

					if (unfinishedTileCache != null)
					{
						await TileCacheRepository.SetTileCacheStarted(unfinishedTileCache.TileCacheId);

						TileCacheManager tileCacheManager = await CreateTileCacheManager(unfinishedTileCache.TileCacheId);

						TilesCount = 0;

						// Do not remove already downloaded tiles
						////tileService.ClearTiles();

						try
						{
							Bounds bounds = GeometryHelper.ToBounds(unfinishedTileCache.Bbox);

							TileSource tileSource = await TileSourceRepository.GetTileSourceWithId(unfinishedTileCache.TileSourceId);

							TileRangeCalculator calculator = new TileRangeCalculator
							{
								ValidBounds = GeometryHelper.ToBounds(tileSource.Bbox),
								ValidZoomRange = new ZoomRange
								{
									MinZoom = 0,
									MaxZoom = tileSource.ZoomLevelMax,
								},
							};

							TileRangeCollection tiles = new TileRangeCollectionCalculator(calculator).GetTileRanges(bounds, new ZoomRange
							{
								MinZoom = unfinishedTileCache.ZoomLevelMin ?? 0,
								MaxZoom = unfinishedTileCache.ZoomLevelMax,
							});

							TilesTotal = tiles.TilesTotal;

							if (TilesTotal > 10000)
							{
								await TileCacheRepository.SetTileCacheError(unfinishedTileCache.TileCacheId);
								throw new ArgumentException($"Tile cache exceeding the maximum number of 10000 tiles: {TilesTotal}.");
							}

							TileDownloader tileDownloader;

							if (tileSource.AllowHiDefStitching)
							{
								tileDownloader = new MergedTileDownloader
								{
									TileServerUrls = tileSource.TileServerUrls.Select(x => x.Url).ToList(),
								};
							}
							else
							{
								tileDownloader = new TileDownloader
								{
									TileServerUrls = tileSource.TileServerUrls.Select(x => x.Url).ToList(),
								};
							}

							await tileDownloader.Download(tiles, (zoomLevel, tileRow, tileColumn, tileData) =>
							{
								// Note that in the TMS tiling scheme, the Y axis is reversed from the "XYZ" coordinate system commonly used in the URLs
								int mbtilesRow = (int)Math.Pow(2, zoomLevel) - 1 - tileRow;

								lock (OfflineCacheBackgroundTask.lockObject)
								{
									TilesCount++;
								}

								if (tileCacheManager.TryGetTile(tileColumn, mbtilesRow, zoomLevel) == null)
								{
									tileCacheManager.SaveTile(zoomLevel, mbtilesRow, tileColumn, tileData);
								}
							});

							Bounds tileCacheBounds = GeometryHelper.ToBounds(unfinishedTileCache.Bbox);
							string metadataBounds = tileCacheBounds.ToCsv();

							double centerLon = tileCacheBounds.Center().Lon;
							double centerLat = tileCacheBounds.Center().Lat;
							string metadataCenter = $"{centerLon},{centerLat},{unfinishedTileCache.ZoomLevelMin}";
							string format = tileSource.TileServerUrls[0].Url.EndsWith("png") ? "png" : "jpg";

							tileCacheManager.SaveMetadata(unfinishedTileCache.Name, format, metadataBounds, metadataCenter,
								unfinishedTileCache.ZoomLevelMin ?? 0, unfinishedTileCache.ZoomLevelMax);

							await TileCacheRepository.SetTileCacheFinished(unfinishedTileCache.TileCacheId,
								$"{unfinishedTileCache.TileCacheId}.mbtiles");
						}
						catch (Exception exception)
						{
							Console.WriteLine(exception);
							Logger.LogWarning(exception,
								$"OfflineCacheBackgroundTask: Exception when creating tile cache for campaign {unfinishedTileCache.TileCacheId}.");
						}
					}

					await Task.Delay(10 * 1000, stoppingToken);
				}
				while (!stoppingToken.IsCancellationRequested);
			}
		}

		private async Task<TileCacheManager> CreateTileCacheManager(Guid tileCacheId)
		{
			string path = Path.Combine(ServiceOptions.DirectoryRoot, "TileCaches", $"{tileCacheId}.mbtiles");

			DbContextOptionsBuilder<TilesContext> dbContextOptionsBuilder = new DbContextOptionsBuilder<TilesContext>();
			dbContextOptionsBuilder.UseSqlite($"Filename={path}");

			TilesContext tilesContext = new TilesContext(dbContextOptionsBuilder.Options);
			await tilesContext.Database.EnsureCreatedAsync();

			return new TileCacheManager(tilesContext);
		}
	}
}