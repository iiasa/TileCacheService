// <copyright file="TileRangeCollection.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Processing.Models
{
	using System.Collections.Generic;
	using System.Linq;

	public class TileRangeCollection
	{
		public IEnumerable<TileIndex> TileIndexes => TileRanges.SelectMany(x => x.TileIndexes);

		public int TilesTotal => TileRanges.Sum(x => x.TilesTotal);

		public IList<TileRange> TileRanges { get; set; } = new List<TileRange>();
	}
}