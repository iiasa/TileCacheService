// <copyright file="CreateTileCacheViewModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Models
{
	using System;

	public class CreateTileCacheViewModel
	{
		public string Bbox { get; set; }

		public string Name { get; set; }

		public Guid TileSourceId { get; set; }

		public int ZoomLevelMax { get; set; }

		public int? ZoomLevelMin { get; set; }
	}
}