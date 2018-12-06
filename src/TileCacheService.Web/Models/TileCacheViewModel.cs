// <copyright file="TileCacheViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Models
{
	using System;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using TileCacheService.Shared.Enums;

#pragma warning disable SA1623 // Property summary documentation should match accessors
#pragma warning disable SA1629 // Documentation text should end with a period
	public class TileCacheViewModel
	{
		/// <summary>
		///     Bounding box for tile cache in WKT representation
		/// </summary>
		/// <example>POLYGON((16.32 48.05, 16.32 48.08, 16.38 48.08, 16.38 48.05, 16.32 48.05))</example>
		public string Bbox { get; set; }

		/// <summary>
		/// Size of the tile cache file in bytes
		/// </summary>
		/// <example>123456</example>
		public long FileSize { get; set; }

		/// <summary>
		///     Name of the created TileCache
		/// </summary>
		/// <example>OSM tile cache for Vienna</example>
		public string Name { get; set; }

		/// <summary>
		/// Status of the tile cache genertation (New, Processing, Finished, or Error)
		/// </summary>
		/// <example>Finished</example>
		[JsonConverter(typeof(StringEnumConverter))]
		public TileCacheStatusEnum Status { get; set; }

		/// <summary>
		///     ID of the tile cache
		/// </summary>
		/// <example>7c9e6679-7425-40de-944b-e07fc1f90ae7</example>
		public Guid TileCacheId { get; set; }

		/// <summary>
		///     ID of the tile source
		/// </summary>
		/// <example>0f8fad5b-d9cb-469f-a165-70867728950e</example>
		public Guid TileSourceId { get; set; }

		/// <summary>
		///     Maximum zoom level where to stop processing the offline cache. Should be lower or equal the maximum zoom level, the
		///     tile source provides
		/// </summary>
		/// <example>20</example>
		public int ZoomLevelMax { get; set; }

		/// <summary>
		///     Optional minimum zoom level where to to start processing. Defaults to 0
		/// </summary>
		/// <example>5</example>
		public int? ZoomLevelMin { get; set; }
	}
#pragma warning restore SA1623 // Property summary documentation should match accessors
#pragma warning restore SA1629 // Documentation text should end with a period
}