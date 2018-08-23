// <copyright file="MergedTileDownloader.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.IO;
	using System.Net.Http;
	using System.Threading.Tasks;
	using SixLabors.ImageSharp;
	using SixLabors.ImageSharp.Formats.Jpeg;
	using SixLabors.ImageSharp.PixelFormats;
	using SixLabors.ImageSharp.Processing;
	using SixLabors.ImageSharp.Processing.Drawing;
	using SixLabors.ImageSharp.Processing.Overlays;
	using TileCacheService.Processing.Models;

	public class MergedTileDownloader : TileDownloader
	{
		public new Task Download(TileRangeCollection tileRangeCollection, Action<int, int, int, byte[]> tileReceivedCallback)
		{
			BlockingCollection<MergedTile> tiles = new BlockingCollection<MergedTile>(100);

			Task producerTask = Task.Run(() =>
				{
					int index = 0;

					foreach (TileRange tileRange in tileRangeCollection.TileRanges)
					{
						foreach (TileIndex tileIndex in tileRange.TileIndexes)
						{
							tiles.Add(new MergedTile()
							{
								ZoomLevel = tileRange.ZoomLevel,
								TileRow = tileIndex.TileRow,
								TileColumn = tileIndex.TileColumn,
								Urls = new[]
								{
									string.Format(TileServerUrls[index % TileServerUrls.Count], tileRange.ZoomLevel + 1,
										tileIndex.TileColumn * 2, tileIndex.TileRow * 2),
									string.Format(TileServerUrls[index % TileServerUrls.Count], tileRange.ZoomLevel + 1,
										(tileIndex.TileColumn * 2) + 1, tileIndex.TileRow * 2),
									string.Format(TileServerUrls[index % TileServerUrls.Count], tileRange.ZoomLevel + 1,
										tileIndex.TileColumn * 2, (tileIndex.TileRow * 2) + 1),
									string.Format(TileServerUrls[index % TileServerUrls.Count], tileRange.ZoomLevel + 1,
										(tileIndex.TileColumn * 2) + 1, (tileIndex.TileRow * 2) + 1),
								},
							});

							index++;
						}
					}
				})
				.ContinueWith(task => tiles.CompleteAdding());

			IList<Task> tasks = new List<Task>();

			for (int i = 0; i < 8; i++)
			{
				Task task = Task.Run(async () =>
				{
					using (HttpClient httpClient = new HttpClient())
					{
						foreach (MergedTile tile in tiles.GetConsumingEnumerable())
						{
							async Task<byte[]> NewFunction(string url, HttpClient httpClient2)
							{
								using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
								{
									request.Headers.Add("user-agent",
										"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36");
									HttpResponseMessage httpResponseMessage = await httpClient2.SendAsync(request);

									if (!httpResponseMessage.IsSuccessStatusCode)
									{
										using (Image<Rgba32> image = new Image<Rgba32>(512, 512))
										{
											image.Mutate(x => x.BackgroundColor(new Rgba32(253, 253, 253)));

											using (MemoryStream memoryStream = new MemoryStream())
											{
												if (url.EndsWith("png"))
												{
													image.SaveAsPng(memoryStream);
												}
												else
												{
													image.SaveAsJpeg(memoryStream);
												}

												return memoryStream.ToArray();
											}
										}
									}

									using (Stream contentStream = await httpResponseMessage.Content.ReadAsStreamAsync())
									using (MemoryStream memoryStream = new MemoryStream())
									{
										await contentStream.CopyToAsync(memoryStream);
										return memoryStream.ToArray();
									}
								}
							}

							byte[] topLeft = await NewFunction(tile.Urls[0], httpClient);
							byte[] topRight = await NewFunction(tile.Urls[1], httpClient);
							byte[] bottomLeft = await NewFunction(tile.Urls[2], httpClient);
							byte[] bottomRight = await NewFunction(tile.Urls[3], httpClient);

							using (Image<Rgba32> image = new Image<Rgba32>(512, 512))
							{
								image.Mutate(o =>
								{
									using (Image<Rgba32> topLeftImage = Image.Load(topLeft, new JpegDecoder()))
									using (Image<Rgba32> topRightImage = Image.Load(topRight, new JpegDecoder()))
									using (Image<Rgba32> bottomLeftImage = Image.Load(bottomLeft, new JpegDecoder()))
									using (Image<Rgba32> bottomRightImage = Image.Load(bottomRight, new JpegDecoder()))
									{
										o.DrawImage(topLeftImage, 1, new SixLabors.Primitives.Point(0, 0));
										o.DrawImage(topRightImage, 1, new SixLabors.Primitives.Point(256, 0));
										o.DrawImage(bottomLeftImage, 1, new SixLabors.Primitives.Point(0, 256));
										o.DrawImage(bottomRightImage, 1, new SixLabors.Primitives.Point(256, 256));
									}
								});

								using (MemoryStream memoryStream = new MemoryStream())
								{
									image.SaveAsJpeg(memoryStream);
									tileReceivedCallback(tile.ZoomLevel, tile.TileRow, tile.TileColumn, memoryStream.ToArray());
								}
							}
						}
					}
				});

				tasks.Add(task);
			}

			return Task.WhenAll(tasks);
		}
	}
}