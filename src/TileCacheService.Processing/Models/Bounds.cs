// <copyright file="Bounds.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing.Models
{
	using System;

	public class Bounds
	{
		public double Bottom { get; set; }

		public double Left { get; set; }

		public double Right { get; set; }

		public double Top { get; set; }

		public (double Lat, double Lon) Center()
		{
			return (Bottom + (Math.Abs(Top - Bottom) / 2), Left + (Math.Abs(Right - Left) / 2));
		}

		public bool Contains(Point point)
		{
			return Top >= point.Y && point.Y >= Bottom && Right >= point.X && point.X >= Left;
		}

		public string ToCsv()
		{
			return $"{Left},{Bottom},{Right},{Top}";
		}
	}
}