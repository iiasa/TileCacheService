// <copyright file="TileServerUrl.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data.Entities
{
	using System;

	public class TileServerUrl
	{
		public int TileServerUrlId { get; set; }

		public TileSource TileSource { get; set; }

		public Guid TileSourceId { get; set; }

		public string Url { get; set; }
	}
}