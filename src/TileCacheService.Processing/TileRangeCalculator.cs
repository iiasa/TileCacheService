// <copyright file="TileRangeCalculator.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing
{
	using System;
	using TileCacheService.Processing.Models;

	public class TileRangeCalculator
	{
		// Defaults to World Bounds
		public Bounds ValidBounds { get; set; } = new Bounds()
		{
			Top = 90,
			Right = 180,
			Bottom = -90,
			Left = -180,
		};

		public ZoomRange ValidZoomRange { get; set; }

		public TileRange GetTiles(Bounds bounds, int zoomLevel)
		{
			if (!ValidBounds.Contains(new Point
			{
				X = bounds.Left,
				Y = bounds.Bottom,
			}) || !bounds.Contains(new Point
			{
				X = bounds.Right,
				Y = bounds.Top,
			}))
			{
				throw new ArgumentException($"The given bounds are not within {nameof(ValidBounds)}.", nameof(bounds));
			}

			if (!ValidZoomRange.Contains(zoomLevel))
			{
				throw new ArgumentException($"The given zoomLevel is not within {nameof(ValidZoomRange)}.", nameof(zoomLevel));
			}

			return new TileRange
			{
				TopLeft = CalculateTileIndex(bounds.Left, bounds.Top, 1 << zoomLevel, 1 << zoomLevel),
				BottomRight = CalculateTileIndex(bounds.Right, bounds.Bottom, 1 << zoomLevel, 1 << zoomLevel),
				ZoomLevel = zoomLevel,
			};
		}

		protected TileIndex CalculateTileIndex(double longitude, double latitude, int matrixWidth, int matrixHeight)
		{
			// See http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames#C.23
			int x = (int)((longitude + 180.0) / 360.0 * matrixWidth);
			int y =
				(int)((1.0 - (Math.Log(Math.Tan(latitude * Math.PI / 180.0) + (1.0 / Math.Cos(latitude * Math.PI / 180.0))) / Math.PI)) /
					2.0 * matrixHeight);

			return new TileIndex
			{
				TileColumn = x,
				TileRow = y,
			};
		}
	}
}