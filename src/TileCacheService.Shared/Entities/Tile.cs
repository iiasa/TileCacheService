// <copyright file="Tile.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared.Entities
{
	// https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md
	public class Tile
	{
		public int TileColumn { get; set; }

		public byte[] TileData { get; set; }

		public int TileRow { get; set; }

		public int ZoomLevel { get; set; }
	}
}