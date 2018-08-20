// <copyright file="TileCacheManager.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared.Services
{
	using System.Linq;
	using TileCacheService.Shared.Entities;

	public class TileCacheManager : ITileCacheManager
	{
		public TileCacheManager(TilesContext tilesContext)
		{
			TilesContext = tilesContext;
		}

		protected TilesContext TilesContext { get; set; }

		public void ClearTiles()
		{
			lock (TilesContext)
			{
				TilesContext.Tiles.RemoveRange(TilesContext.Tiles.ToList());
				TilesContext.SaveChanges();
			}
		}

		public int GetTilesCount()
		{
			lock (TilesContext)
			{
				return TilesContext.Tiles.Count();
			}
		}

		public void SaveTile(int zoomLevel, int tileRow, int tileColumn, byte[] tileData)
		{
			lock (TilesContext)
			{
				TilesContext.Tiles.Add(new Tile()
				{
					ZoomLevel = zoomLevel,
					TileColumn = tileColumn,
					TileRow = tileRow,
					TileData = tileData,
				});
				TilesContext.SaveChanges();
			}
		}

		public void SaveMetadata(string name, string format, string bounds, string center, int minZoom, int maxZoom)
		{
			lock (TilesContext)
			{
				TilesContext.Metadata.Add(new Metadata { Name = "name", Text = name });
				TilesContext.Metadata.Add(new Metadata { Name = "format", Text = format });
				TilesContext.Metadata.Add(new Metadata { Name = "bounds", Text = bounds });
				TilesContext.Metadata.Add(new Metadata { Name = "center", Text = center });
				TilesContext.Metadata.Add(new Metadata { Name = "minzoom", Text = minZoom.ToString() });
				TilesContext.Metadata.Add(new Metadata { Name = "maxzoom", Text = maxZoom.ToString() });
				TilesContext.SaveChanges();
			}
		}

		public Tile TryGetTile(int tileColumn, int tileRow, int zoomLevel)
		{
			lock (TilesContext)
			{
				return TilesContext.Tiles.SingleOrDefault(tile =>
					tile.ZoomLevel == zoomLevel && tile.TileColumn == tileColumn && tile.TileRow == tileRow);
			}
		}
	}
}