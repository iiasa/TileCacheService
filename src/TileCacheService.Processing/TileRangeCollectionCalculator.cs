// <copyright file="TileRangeCollectionCalculator.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing
{
	using TileCacheService.Processing.Models;

	public class TileRangeCollectionCalculator
	{
		public TileRangeCollectionCalculator(TileRangeCalculator tileRangeCalculator)
		{
			TileRangeCalculator = tileRangeCalculator;
		}

		private TileRangeCalculator TileRangeCalculator { get; }

		public TileRangeCollection GetTileRanges(Bounds bounds, ZoomRange zoomLevel)
		{
			TileRangeCollection tileRangeCollection = new TileRangeCollection();

			for (int i = zoomLevel.MinZoom; i <= zoomLevel.MaxZoom; i++)
			{
				tileRangeCollection.TileRanges.Add(TileRangeCalculator.GetTiles(bounds, i));
			}

			return tileRangeCollection;
		}
	}
}