// <copyright file="20180820131919_InitialModel.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data.Migrations
{
	using System;
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class InitialModel : Migration
	{
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable("tile_caches");

			migrationBuilder.DropTable("tile_server_url");

			migrationBuilder.DropTable("tile_sources");

			migrationBuilder.DropTable("map_type_enum");
		}

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable("map_type_enum", table => new
			{
				id = table.Column<int>(nullable: false),
				name = table.Column<string>(nullable: true),
				value = table.Column<int>(nullable: false),
			}, constraints: table =>
			{
				table.PrimaryKey("pk_map_type_enum", x => x.id);
				table.UniqueConstraint("ak_map_type_enum_value", x => x.value);
			});

			migrationBuilder.CreateTable("tile_sources", table => new
			{
				allow_hi_def_stitching = table.Column<bool>(nullable: false),
				bbox = table.Column<string>(nullable: true),
				map_type = table.Column<int>(nullable: false),
				name = table.Column<string>(nullable: true),
				tile_source_id = table.Column<Guid>(nullable: false),
				zoom_level_max = table.Column<int>(nullable: false),
			}, constraints: table =>
			{
				table.PrimaryKey("pk_tile_sources", x => x.tile_source_id);
				table.ForeignKey("fk_tile_sources_map_type_enum_map_type", x => x.map_type, "map_type_enum", "value",
					onDelete: ReferentialAction.Cascade);
			});

			migrationBuilder.CreateTable("tile_caches", table => new
			{
				bbox = table.Column<string>(nullable: true),
				filename = table.Column<string>(nullable: true),
				name = table.Column<string>(nullable: true),
				processing_finished = table.Column<DateTime>(nullable: true),
				processing_started = table.Column<DateTime>(nullable: true),
				tile_cache_id = table.Column<Guid>(nullable: false),
				tile_source_id = table.Column<Guid>(nullable: false),
				zoom_level_max = table.Column<int>(nullable: false),
				zoom_level_min = table.Column<int>(nullable: true),
			}, constraints: table =>
			{
				table.PrimaryKey("pk_tile_caches", x => x.tile_cache_id);
				table.ForeignKey("fk_tile_caches_tile_sources_tile_source_id", x => x.tile_source_id, "tile_sources", "tile_source_id",
					onDelete: ReferentialAction.Cascade);
			});

			migrationBuilder.CreateTable("tile_server_url", table => new
			{
				tile_server_url_id = table.Column<int>(nullable: false).Annotation("Sqlite:Autoincrement", true),
				tile_source_id = table.Column<Guid>(nullable: false),
				url = table.Column<string>(nullable: true),
			}, constraints: table =>
			{
				table.PrimaryKey("pk_tile_server_url", x => x.tile_server_url_id);
				table.ForeignKey("fk_tile_server_url_tile_sources_tile_source_id", x => x.tile_source_id, "tile_sources", "tile_source_id",
					onDelete: ReferentialAction.Cascade);
			});

			migrationBuilder.InsertData("map_type_enum", new[] { "id", "name", "value" }, new object[] { 1, "Aerial", 1 });

			migrationBuilder.InsertData("map_type_enum", new[] { "id", "name", "value" }, new object[] { 2, "Road", 2 });

			migrationBuilder.InsertData("map_type_enum", new[] { "id", "name", "value" }, new object[] { 3, "Terrain", 3 });

			migrationBuilder.InsertData("map_type_enum", new[] { "id", "name", "value" }, new object[] { 4, "Hybrid", 4 });

			migrationBuilder.InsertData("map_type_enum", new[] { "id", "name", "value" }, new object[] { 5, "Other", 5 });

#pragma warning disable SA1118 // Parameter should not span multiple lines
			migrationBuilder.InsertData("tile_sources",
				new[] { "tile_source_id", "allow_hi_def_stitching", "bbox", "map_type", "name", "zoom_level_max" },
				new object[]
				{
					new Guid("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"), true,
					"POLYGON((8.782379 46.358770, 8.782379 49.037872, 17.5 49.037872, 17.5 46.358770, 8.782379 46.358770))", 1,
					"basemap.at orthofoto30cm", 20,
				});

			migrationBuilder.InsertData("tile_sources",
				new[] { "tile_source_id", "allow_hi_def_stitching", "bbox", "map_type", "name", "zoom_level_max" },
				new object[]
				{
					new Guid("4a91da65-c466-4d88-ace2-b4ff49d44d3c"), false, "POLYGON((-180 -90, 180 -90, 180 90, -180 90, -180 -90))", 2,
					"OpenStreetMap Standard layer", 20,
				});

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[]
				{
					1, new Guid("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					"http://maps.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				});

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[]
				{
					2, new Guid("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					"http://maps1.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				});

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[]
				{
					3, new Guid("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					"http://maps2.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				});

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[]
				{
					4, new Guid("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					"http://maps3.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				});

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[]
				{
					5, new Guid("f9168c5e-ceb2-4aaa-b6bf-329bf39fa1e4"),
					"http://maps4.wien.gv.at/basemap/bmaporthofoto30cm/normal/google3857/{0}/{2}/{1}.jpg",
				});
#pragma warning restore SA1118 // Parameter should not span multiple lines

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[] { 6, new Guid("4a91da65-c466-4d88-ace2-b4ff49d44d3c"), "https://a.tile.openstreetmap.org/{0}/{1}/{2}.png" });

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[] { 7, new Guid("4a91da65-c466-4d88-ace2-b4ff49d44d3c"), "https://b.tile.openstreetmap.org/{0}/{1}/{2}.png" });

			migrationBuilder.InsertData("tile_server_url", new[] { "tile_server_url_id", "tile_source_id", "url" },
				new object[] { 8, new Guid("4a91da65-c466-4d88-ace2-b4ff49d44d3c"), "https://c.tile.openstreetmap.org/{0}/{1}/{2}.png" });

			migrationBuilder.CreateIndex("ix_tile_caches_tile_source_id", "tile_caches", "tile_source_id");

			migrationBuilder.CreateIndex("ix_tile_server_url_tile_source_id", "tile_server_url", "tile_source_id");

			migrationBuilder.CreateIndex("ix_tile_sources_map_type", "tile_sources", "map_type");
		}
	}
}