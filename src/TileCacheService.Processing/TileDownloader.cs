// <copyright file="TileDownloader.cs" company="IIASA">
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
	using TileCacheService.Processing.Models;

	public class TileDownloader
	{
		public IList<string> TileServerUrls { get; set; }

		public Task Download(TileRangeCollection tileRangeCollection, Action<int, int, int, byte[]> tileReceivedCallback)
		{
			BlockingCollection<Tile> tiles = new BlockingCollection<Tile>(100);

			Task producerTask = Task.Run(() =>
				{
					int index = 0;

					foreach (TileRange tileRange in tileRangeCollection.TileRanges)
					{
						foreach (TileIndex tileIndex in tileRange.TileIndexes)
						{
							tiles.Add(new Tile()
							{
								ZoomLevel = tileRange.ZoomLevel,
								TileRow = tileIndex.TileRow,
								TileColumn = tileIndex.TileColumn,
								Url = string.Format(TileServerUrls[index % TileServerUrls.Count], tileRange.ZoomLevel, tileIndex.TileColumn,
									tileIndex.TileRow),
							});

							index++;
						}
					}
				})
				.ContinueWith(task => tiles.CompleteAdding());

			IList<Task> tasks = new List<Task>();

			for (int i = 0; i < 4; i++)
			{
				Task task = Task.Run(async () =>
				{
					using (HttpClient client = new HttpClient())
					{
						foreach (Tile tile in tiles.GetConsumingEnumerable())
						{
							using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, tile.Url))
							using (Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync())
							using (MemoryStream memoryStream = new MemoryStream())
							{
								await contentStream.CopyToAsync(memoryStream);
								tileReceivedCallback(tile.ZoomLevel, tile.TileRow, tile.TileColumn, memoryStream.ToArray());
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