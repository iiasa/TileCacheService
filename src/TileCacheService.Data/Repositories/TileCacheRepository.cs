// <copyright file="TileCacheRepository.cs" company="IIASA">
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

	public class TileCacheRepository
	{
		public TileCacheRepository(TileCacheServiceContext context)
		{
			Context = context;
		}

		public TileCacheServiceContext Context { get; set; }

		public async Task<TileCache> CreateTileCache(TileCache tileCache)
		{
			await Context.TileCaches.AddAsync(tileCache);
			await Context.SaveChangesAsync();

			return tileCache;
		}

		public async Task<TileCache> GetFirstUnfinishedTileCache()
		{
			return await Context.TileCaches.FirstOrDefaultAsync(x =>
				!x.ProcessingStarted.HasValue ||
				(!x.ProcessingFinished.HasValue && (x.ProcessingStarted.Value - DateTime.Now) < TimeSpan.FromHours(1)));
		}

		public async Task<List<TileCache>> GetTileCaches()
		{
			return await Context.TileCaches.ToListAsync();
		}

		public async Task<TileCache> GetTileCacheWithId(Guid id)
		{
			return await Context.TileCaches.SingleOrDefaultAsync(x => x.TileCacheId == id);
		}

		public async Task SetTileCacheFinished(Guid id, string fileName)
		{
			TileCache tileCache = await Context.TileCaches.SingleAsync(x => x.TileCacheId == id);
			tileCache.ProcessingFinished = DateTime.Now;
			tileCache.Filename = fileName;

			await Context.SaveChangesAsync();
		}

		public async Task SetTileCacheStarted(Guid id)
		{
			TileCache tileCache = await Context.TileCaches.SingleAsync(x => x.TileCacheId == id);
			tileCache.ProcessingStarted = DateTime.Now;

			await Context.SaveChangesAsync();
		}
	}
}