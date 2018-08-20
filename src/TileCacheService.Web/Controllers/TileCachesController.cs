// <copyright file="TileCachesController.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using AutoMapper;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Net.Http.Headers;
	using TileCacheService.Data.Entities;
	using TileCacheService.Data.Repositories;
	using TileCacheService.Web.Models;

	[Route("v1/[controller]")]
	[ApiController]
	public class TileCachesController : ControllerBase
	{
		public TileCachesController(IMapper mapper, TileCacheRepository tileCacheRepository)
		{
			Mapper = mapper;
			TileCacheRepository = tileCacheRepository;
		}

		public IMapper Mapper { get; set; }

		public TileCacheRepository TileCacheRepository { get; set; }

		[HttpGet("{tileCacheId}/Download")]
		[Produces(@"application/octet-stream")]
		public async Task<IActionResult> Download(Guid tileCacheId)
		{
			TileCache tileCache = await TileCacheRepository.GetTileCacheWithId(tileCacheId);

			if (tileCache == null || !System.IO.File.Exists(Path.Combine("TileCaches", tileCache.Filename)))
			{
				return NotFound();
			}

			// Set up the content-disposition header with proper encoding of the filename
			ContentDispositionHeaderValue contentDisposition = new ContentDispositionHeaderValue("attachment");
			contentDisposition.SetHttpFileName(tileCache.Filename);
			Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

			FileStream stream = new FileStream(Path.Combine("TileCaches", tileCache.Filename), FileMode.Open);
			return new FileStreamResult(stream, "application/octet-stream");
		}

		[HttpGet]
		[ProducesResponseType(typeof(TileCacheViewModel), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get()
		{
			List<TileCache> tileSources = await TileCacheRepository.GetTileCaches();

			return Ok(tileSources.Select(x => Mapper.Map<TileCacheViewModel>(x)));
		}

		[HttpGet("{tileCacheId}")]
		[ProducesResponseType(typeof(TileCacheViewModel), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get(Guid tileCacheId)
		{
			TileCache tileCache = await TileCacheRepository.GetTileCacheWithId(tileCacheId);

			if (tileCache == null)
			{
				return NotFound();
			}

			return Ok(Mapper.Map<TileCacheViewModel>(tileCache));
		}

		[HttpPost]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Post([FromBody] CreateTileCacheViewModel viewModel)
		{
			TileCache tileCache = await TileCacheRepository.CreateTileCache(Mapper.Map<TileCache>(viewModel));

			return CreatedAtAction(nameof(Get), new
			{
				tileCacheId = tileCache.TileCacheId,
			}, null);
		}
	}
}