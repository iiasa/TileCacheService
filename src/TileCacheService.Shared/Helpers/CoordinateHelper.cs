// <copyright file="CoordinateHelper.cs" company="IIASA">
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

	////using GeoJSON.Net.Geometry;

	public static class CoordinateHelper
	{
		public static string BoundsToWkt(Bounds bounds)
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

		public static string CoordinateToWktString(double lat, double lon)
		{
			return new Wkx.Point(lon, lat).SerializeString<WktSerializer>();
		}

		////public static void ExtendBounds(this Bounds bounds, int km)
		////{
		////	bounds.Left -= 0.0135 * km;
		////	bounds.Bottom -= 0.009 * km;
		////	bounds.Right += 0.0135 * km;
		////	bounds.Top += 0.009 * km;
		////}

		////public static List<PointViewModel> WktStringToLineViewModel(string wktString)
		////{
		////	Geometry geometry = Geometry.Deserialize<WktSerializer>(wktString);

		////	if (geometry.GeometryType == GeometryType.LineString)
		////	{
		////		Wkx.LineString line = geometry as Wkx.LineString;

		////		return line.Points.Select(x => new PointViewModel()
		////			{
		////				Longitude = x.X.Value,
		////				Latitude = x.Y.Value,
		////			})
		////			.ToList();
		////	}

		////	throw new ArgumentException("WKT String couldn't be converted to position.", nameof(wktString));
		////}

		////public static GeoJSON.Net.Geometry.Point WktStringToPoint(string wktString)
		////{
		////	Geometry geometry = Geometry.Deserialize<WktSerializer>(wktString);

		////	if (geometry.GeometryType == GeometryType.Point)
		////	{
		////		Wkx.Point p = geometry as Wkx.Point;

		////		return new GeoJSON.Net.Geometry.Point(new Position(p.Y.Value, p.X.Value));
		////	}

		////	throw new ArgumentException("WKT String couldn't be converted to point.", nameof(wktString));
		////}

		////public static PointViewModel WktStringToPointViewModel(string wktString)
		////{
		////	Geometry geometry = Geometry.Deserialize<WktSerializer>(wktString);

		////	if (geometry.GeometryType == GeometryType.Point)
		////	{
		////		Wkx.Point p = geometry as Wkx.Point;

		////		return new PointViewModel()
		////		{
		////			Longitude = p.X.Value,
		////			Latitude = p.Y.Value,
		////		};
		////	}

		////	throw new ArgumentException("WKT String couldn't be converted to point.", nameof(wktString));
		////}

		public static List<PointViewModel> WktStringToPolygonViewModel(string wktString)
		{
			Geometry geometry = Geometry.Deserialize<WktSerializer>(wktString);

			if (geometry.GeometryType == GeometryType.Polygon)
			{
				Polygon polygon = geometry as Polygon;

				return polygon.ExteriorRing.Points.Select(x => new PointViewModel()
					{
						Longitude = x.X.Value,
						Latitude = x.Y.Value,
					})
					.ToList();
			}

			throw new ArgumentException("WKT String couldn't be converted to polygon.", nameof(wktString));
		}

		public static Bounds WktToBounds(string wkt)
		{
			List<PointViewModel> points = WktStringToPolygonViewModel(wkt);

			return new Bounds
			{
				Left = points.Min(x => x.Longitude),
				Right = points.Max(x => x.Longitude),
				Bottom = points.Min(x => x.Latitude),
				Top = points.Max(x => x.Latitude),
			};
		}
	}
}