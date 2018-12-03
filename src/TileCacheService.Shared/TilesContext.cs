// <copyright file="TilesContext.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared
{
	using Microsoft.EntityFrameworkCore;
	using SpatialFocus.EntityFrameworkCore.Extensions;
	using TileCacheService.Shared.Entities;

	public class TilesContext : DbContext
	{
		public TilesContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Metadata> Metadata { get; set; }

		public DbSet<Tile> Tiles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Tile>()
				.HasKey(x => new
				{
					x.ZoomLevel,
					x.TileColumn,
					x.TileRow,
				});

			modelBuilder.ConfigureNames(NamingOptions.Default.SetNamingScheme(NamingScheme.SnakeCase)
				.SetTableNamingSource(NamingSource.DbSet));
		}
	}
}