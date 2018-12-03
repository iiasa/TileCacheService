// <copyright file="TileCacheServiceContext.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data
{
	using System;
	using Microsoft.EntityFrameworkCore;
	using SpatialFocus.EntityFrameworkCore.Extensions;
	using TileCacheService.Data.Entities;
	using TileCacheService.Shared.Enums;

	public class TileCacheServiceContext : DbContext
	{
		public TileCacheServiceContext(DbContextOptions<TileCacheServiceContext> options)
			: base(options)
		{
			Database.Migrate();
		}

		public TileCacheServiceContext()
		{
			Database.Migrate();
		}

		public DbSet<TileCache> TileCaches { get; set; }

		public DbSet<TileSource> TileSources { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ConfigureEnumLookup(EnumLookupOptions.Default);

			modelBuilder.ConfigureNames(NamingOptions.Default.SkipTableNamingForGenericEntityTypes()
				.SetTableNamingSource(NamingSource.DbSet));

			modelBuilder.Entity<TileSource>()
				.HasData(new TileSource
				{
					TileSourceId = Guid.Parse("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					MapType = MapTypeEnum.Aerial,
					ZoomLevelMax = 20,
					Name = "basemap.at orthofoto30cm",
					Bbox = "POLYGON((8.782379 46.358770, 8.782379 49.037872, 17.5 49.037872, 17.5 46.358770, 8.782379 46.358770))",
					AllowHiDefStitching = true,
				}, new TileSource
				{
					TileSourceId = Guid.Parse("4a91da65-c466-4d88-ace2-b4ff49d44d3c"),
					MapType = MapTypeEnum.Road,
					ZoomLevelMax = 20,
					Name = "OpenStreetMap Standard layer",
					Bbox = "POLYGON((-180 -90, 180 -90, 180 90, -180 90, -180 -90))",
				});

			modelBuilder.Entity<TileServerUrl>()
				.HasData(new TileServerUrl
				{
					TileServerUrlId = 1,
					TileSourceId = Guid.Parse("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					Url = "http://maps.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				}, new TileServerUrl
				{
					TileServerUrlId = 2,
					TileSourceId = Guid.Parse("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					Url = "http://maps1.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				}, new TileServerUrl
				{
					TileServerUrlId = 3,
					TileSourceId = Guid.Parse("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					Url = "http://maps2.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				}, new TileServerUrl
				{
					TileServerUrlId = 4,
					TileSourceId = Guid.Parse("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					Url = "http://maps3.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				}, new TileServerUrl
				{
					TileServerUrlId = 5,
					TileSourceId = Guid.Parse("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					Url = "http://maps4.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				}, new TileServerUrl
				{
					TileServerUrlId = 6,
					TileSourceId = Guid.Parse("4a91da65-c466-4d88-ace2-b4ff49d44d3c"),
					Url = "https://a.tile.openstreetmap.org/{0}/{1}/{2}.png",
				}, new TileServerUrl
				{
					TileServerUrlId = 7,
					TileSourceId = Guid.Parse("4a91da65-c466-4d88-ace2-b4ff49d44d3c"),
					Url = "https://b.tile.openstreetmap.org/{0}/{1}/{2}.png",
				}, new TileServerUrl
				{
					TileServerUrlId = 8,
					TileSourceId = Guid.Parse("4a91da65-c466-4d88-ace2-b4ff49d44d3c"),
					Url = "https://c.tile.openstreetmap.org/{0}/{1}/{2}.png",
				});
		}
	}
}