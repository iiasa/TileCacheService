// <copyright file="ZoomRange.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing.Models
{
	public class ZoomRange
	{
		public int MaxZoom { get; set; }

		public int MinZoom { get; set; }

		public bool Contains(int zoomLevel)
		{
			return MaxZoom >= zoomLevel && zoomLevel >= MinZoom;
		}
	}
}