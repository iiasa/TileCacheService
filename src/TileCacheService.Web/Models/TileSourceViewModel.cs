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

	public class TileSourceViewModel
	{
		public bool AllowHiDefStitching { get; set; }
		public string Bbox { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MapTypeEnum MapType { get; set; }

		public string Name { get; set; }

		public ISet<string> TileServerUrls { get; set; }

		public Guid TileSourceId { get; set; }

		public int ZoomLevelMax { get; set; }
	}
}