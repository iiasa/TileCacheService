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

	public class TileCacheViewModel
	{
		public string Bbox { get; set; }

		public string Name { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public TileCacheStatusEnum Status { get; set; }

		public Guid TileCacheId { get; set; }

		public Guid TileSourceId { get; set; }

		public int ZoomLevelMax { get; set; }

		public int? ZoomLevelMin { get; set; }
	}
}