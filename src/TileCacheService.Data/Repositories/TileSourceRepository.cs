// <copyright file="TileSourceRepository.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Data.Repositories
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using TileCacheService.Data.Entities;

	public class TileSourceRepository
	{
		public TileSourceRepository(TileCacheServiceContext context)
		{
			Context = context;
		}

		public TileCacheServiceContext Context { get; set; }

		public async Task<TileSource> CreateTileSource(TileSource tileSource)
		{
			await Context.TileSources.AddAsync(tileSource);

			await Context.SaveChangesAsync();

			return tileSource;
		}

		public async Task<List<TileSource>> GetTileSources()
		{
			return await Context.TileSources.Include(x => x.TileServerUrls).ToListAsync();
		}

		public async Task<TileSource> GetTileSourceWithId(Guid id)
		{
			return await Context.TileSources.Include(x => x.TileServerUrls).SingleOrDefaultAsync(x => x.TileSourceId == id);
		}
	}
}