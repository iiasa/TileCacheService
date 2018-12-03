// <copyright file="TileSource.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data.Entities
{
	using System;
	using System.Collections.Generic;
	using TileCacheService.Shared.Enums;

	public class TileSource
	{
		public bool AllowHiDefStitching { get; set; }

		public string Bbox { get; set; }

		public MapTypeEnum MapType { get; set; }

		public string Name { get; set; }

		public virtual List<TileServerUrl> TileServerUrls { get; set; }

		public Guid TileSourceId { get; set; }

		public int ZoomLevelMax { get; set; }
	}
}