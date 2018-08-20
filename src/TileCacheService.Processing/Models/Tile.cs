// <copyright file="Tile.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing.Models
{
	public class Tile
	{
		public int TileColumn { get; set; }

		public int TileRow { get; set; }

		public string Url { get; set; }

		public int ZoomLevel { get; set; }
	}
}