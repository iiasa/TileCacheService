// <copyright file="CreateTileSourceViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Models
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using TileCacheService.Shared.Enums;

#pragma warning disable SA1623 // Property summary documentation should match accessors
#pragma warning disable SA1629 // Documentation text should end with a period
	public class CreateTileSourceViewModel
	{
		/// <summary>
		///     Indicate whether 4 tiles from a higher zoom level should be stitched together and resampled to increase the image
		///     quality
		/// </summary>
		/// <example>false</example>
		public bool AllowHiDefStitching { get; set; }

		/// <summary>
		///     Bounding box for tile source. Either pass WKT polygon or lon,lat,lon,lat (lower left, upper right)
		/// </summary>
		/// <example>16,47,17,48</example>
		public string Bbox { get; set; }

		/// <summary>
		///     Type of the Tile Source imagery (Aerial, Road, Terrain, Hybrid, or Other)
		/// </summary>
		/// <example>Road</example>
		[JsonConverter(typeof(StringEnumConverter))]
		public MapTypeEnum MapType { get; set; }

		/// <summary>
		///     Name of the Tile Source
		/// </summary>
		/// <example>OSM tiles</example>
		public string Name { get; set; }

		/// <summary>
		///     Link(s) to the tile server, containing {0}, {1}, and {2} as placeholders for zoom, column, and row
		/// </summary>
		/// <example>https://a.tile.openstreetmap.org/{0}/{1}/{2}.png</example>
		public ISet<string> TileServerUrls { get; set; }

		/// <summary>
		///     Maximum zoom level that this tile source provides (if AllowHiDefStitching should be used, decrease by 1)
		/// </summary>
		/// <example>20</example>
		public int ZoomLevelMax { get; set; }
	}
#pragma warning restore SA1623 // Property summary documentation should match accessors
#pragma warning restore SA1629 // Documentation text should end with a period
}