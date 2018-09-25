// <copyright file="GeometryHelper.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using TileCacheService.Processing.Models;
	using TileCacheService.Shared.ViewModels;
	using Wkx;

	public static class GeometryHelper
	{
		public static string HarmonizeBbox(string input)
		{
			string[] inputSplit = input.Trim().Split(',');

			if (inputSplit.Length == 4)
			{
				// Format seems to be lon,lat,lon,lat (lower left, upper right)
				Bounds bounds = new Bounds
				{
					Left = double.Parse(inputSplit[0]),
					Bottom = double.Parse(inputSplit[1]),
					Right = double.Parse(inputSplit[2]),
					Top = double.Parse(inputSplit[3]),
				};

				return ToWktPolygon(bounds);
			}

			return input;
		}

		public static Bounds ToBounds(string wktPolygon)
		{
			Geometry geometry = Geometry.Deserialize<WktSerializer>(wktPolygon);

			if (geometry.GeometryType == GeometryType.Polygon)
			{
				Polygon polygon = geometry as Polygon;

				// ReSharper disable once PossibleNullReferenceException
				List<PointViewModel> points = polygon.ExteriorRing.Points.Select(x => new PointViewModel()
					{
						Longitude = x.X.Value,
						Latitude = x.Y.Value,
					})
					.ToList();

				return new Bounds
				{
					Left = points.Min(x => x.Longitude),
					Right = points.Max(x => x.Longitude),
					Bottom = points.Min(x => x.Latitude),
					Top = points.Max(x => x.Latitude),
				};
			}

			throw new ArgumentException("WKT String couldn't be converted to polygon.", nameof(wktPolygon));
		}

		public static string ToWktPolygon(Bounds bounds)
		{
			Geometry geometry = new Polygon(new List<Wkx.Point>()
			{
				new Wkx.Point(bounds.Left, bounds.Bottom),
				new Wkx.Point(bounds.Left, bounds.Top),
				new Wkx.Point(bounds.Right, bounds.Top),
				new Wkx.Point(bounds.Right, bounds.Bottom),
				new Wkx.Point(bounds.Left, bounds.Bottom),
			});

			return geometry.SerializeString<WktSerializer>();
		}

		////public static void ExtendBounds(this Bounds bounds, int km)
		////{
		////	bounds.Left -= 0.0135 * km;
		////	bounds.Bottom -= 0.009 * km;
		////	bounds.Right += 0.0135 * km;
		////	bounds.Top += 0.009 * km;
		////}
	}
}