// <copyright file="TileRange.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing.Models
{
	using System.Collections.Generic;

	public class TileRange
	{
		public int TileColumns => BottomRight.TileColumn - TopLeft.TileColumn + 1;

		public IEnumerable<TileIndex> TileIndexes
		{
			get
			{
				for (int i = TopLeft.TileRow; i <= BottomRight.TileRow; i++)
				{
					for (int j = TopLeft.TileColumn; j <= BottomRight.TileColumn; j++)
					{
						yield return new TileIndex
						{
							TileColumn = j,
							TileRow = i,
						};
					}
				}
			}
		}

		public int TileRows => BottomRight.TileRow - TopLeft.TileRow + 1;

		public int TilesTotal => TileColumns * TileRows;

		public TileIndex BottomRight { get; set; }

		public TileIndex TopLeft { get; set; }

		public int ZoomLevel { get; set; }
	}
}