// <copyright file="TileCache.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data.Entities
{
	using System;

	public class TileCache
	{
		public string Bbox { get; set; }

		public string Filename { get; set; }

		public string Name { get; set; }

		public bool ProcessingError { get; set; }

		public DateTime? ProcessingFinished { get; set; }

		public DateTime? ProcessingStarted { get; set; }

		public int RetryCount { get; set; }

		public Guid TileCacheId { get; set; }

		public TileSource TileSource { get; set; }

		public Guid TileSourceId { get; set; }

		public int ZoomLevelMax { get; set; }

		public int? ZoomLevelMin { get; set; }
	}
}