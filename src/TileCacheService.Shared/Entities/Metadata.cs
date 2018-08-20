// <copyright file="Metadata.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared.Entities
{
	using System.ComponentModel.DataAnnotations;

	// https://github.com/mapbox/mbtiles-spec/blob/master/1.3/spec.md
	public class Metadata
	{
		[Key]
		public string Name { get; set; }

		public string Text { get; set; }
	}
}