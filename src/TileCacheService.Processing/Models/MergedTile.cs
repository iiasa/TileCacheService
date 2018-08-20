// <copyright file="MergedTile.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing.Models
{
	public class MergedTile
	{
		public int TileColumn { get; set; }

		public int TileRow { get; set; }

		public string[] Urls { get; set; }

		public int ZoomLevel { get; set; }
	}
}