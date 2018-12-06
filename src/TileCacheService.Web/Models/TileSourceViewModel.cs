// <copyright file="TileSourceViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Models
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using TileCacheService.Shared.Enums;

#pragma warning disable SA1623 // Property summary documentation should match accessors
#pragma warning disable SA1629 // Documentation text should end with a period
	public class TileSourceViewModel
	{
		/// <summary>
		///     If set, 4 tiles from a higher zoom level will be stitched together and resampled to increase the image
		///     quality
		/// </summary>
		/// <example>false</example>
		public bool AllowHiDefStitching { get; set; }

		/// <summary>
		///     Bounding box for tile source in WKT representation
		/// </summary>
		/// <example>POLYGON((16 47, 16 48, 17 48, 17 47, 16 47))</example>
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
		///     ID of the tile source
		/// </summary>
		/// <example>0f8fad5b-d9cb-469f-a165-70867728950e</example>
		public Guid TileSourceId { get; set; }

		/// <summary>
		///     Maximum zoom level that this tile source provides
		/// </summary>
		/// <example>20</example>
		public int ZoomLevelMax { get; set; }
	}
#pragma warning restore SA1623 // Property summary documentation should match accessors
#pragma warning restore SA1629 // Documentation text should end with a period
}