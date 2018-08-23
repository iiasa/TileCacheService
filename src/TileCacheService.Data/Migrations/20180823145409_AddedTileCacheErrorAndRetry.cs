// <copyright file="20180823145409_AddedTileCacheErrorAndRetry.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data.Migrations
{
	using Microsoft.EntityFrameworkCore.Migrations;

	public partial class AddedTileCacheErrorAndRetry : Migration
	{
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn("processing_error", "tile_caches");

			migrationBuilder.DropColumn("retry_count", "tile_caches");
		}

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<bool>("processing_error", "tile_caches", nullable: false, defaultValue: false);

			migrationBuilder.AddColumn<int>("retry_count", "tile_caches", nullable: false, defaultValue: 0);
		}
	}
}