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
	using Microsoft.Extensions.Options;
	using Microsoft.Net.Http.Headers;
	using TileCacheService.Data.Entities;
	using TileCacheService.Data.Repositories;
	using TileCacheService.Web.Core;
	using TileCacheService.Web.Models;

	[Route("v1/[controller]")]
	[ApiController]
	public class TileCachesController : ControllerBase
	{
		public TileCachesController(IMapper mapper, TileCacheRepository tileCacheRepository, IOptions<ServiceOptions> serviceOptions)
		{
			Mapper = mapper;
			TileCacheRepository = tileCacheRepository;
			ServiceOptions = serviceOptions.Value;
		}

		public IMapper Mapper { get; }

		public ServiceOptions ServiceOptions { get; }

		public TileCacheRepository TileCacheRepository { get; }

		[HttpGet("{tileCacheId:Guid}/Download")]
		[Produces(@"application/octet-stream", @"text/plain", @"application/json", @"text/json")]
		[ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Download(Guid tileCacheId)
		{
			TileCache tileCache = await TileCacheRepository.GetTileCacheWithId(tileCacheId);

			if (tileCache == null)
			{
				return NotFound();
			}

			string path = Path.Combine(ServiceOptions.DirectoryRoot, "TileCaches", tileCache.Filename);
			if (!System.IO.File.Exists(path))
			{
				return NotFound();
			}

			// Set up the content-disposition header with proper encoding of the filename
			ContentDispositionHeaderValue contentDisposition = new ContentDispositionHeaderValue("attachment");
			contentDisposition.SetHttpFileName(tileCache.Filename);
			Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

			FileStream stream = new FileStream(path, FileMode.Open);
			return new FileStreamResult(stream, @"application/octet-stream");
		}

		[HttpGet]
		[ProducesResponseType(typeof(TileCacheViewModel), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get()
		{
			List<TileCache> tileSources = await TileCacheRepository.GetTileCaches();

			return Ok(tileSources.Select(x => Mapper.Map<TileCacheViewModel>(x)));
		}

		[HttpGet("{tileCacheId:Guid}")]
		[ProducesResponseType(typeof(TileCacheViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
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
		[ProducesResponseType(typeof(TileCacheViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Post([FromBody] CreateTileCacheViewModel viewModel)
		{
			TileCache tileCache = await TileCacheRepository.CreateTileCache(Mapper.Map<TileCache>(viewModel));

			return CreatedAtAction(nameof(Get), new
			{
				tileCacheId = tileCache.TileCacheId,
			}, Mapper.Map<TileCacheViewModel>(tileCache));
		}
	}
}