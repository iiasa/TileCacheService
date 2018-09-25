// <copyright file="TileSourcesController.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading.Tasks;
	using AutoMapper;
	using Microsoft.AspNetCore.Mvc;
	using TileCacheService.Data.Entities;
	using TileCacheService.Data.Repositories;
	using TileCacheService.Web.Models;

	[Route("v1/[controller]")]
	[ApiController]
	public class TileSourcesController : ControllerBase
	{
		public TileSourcesController(IMapper mapper, TileSourceRepository tileSourceRepository)
		{
			Mapper = mapper;
			TileSourceRepository = tileSourceRepository;
		}

		public IMapper Mapper { get; set; }

		public TileSourceRepository TileSourceRepository { get; set; }

		[HttpGet]
		[ProducesResponseType(typeof(List<TileSourceViewModel>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> Get()
		{
			List<TileSource> tileSources = await TileSourceRepository.GetTileSources();

			return Ok(tileSources.Select(x => Mapper.Map<TileSourceViewModel>(x)));
		}

		[HttpGet("{tileSourceId:Guid}")]
		[ProducesResponseType(typeof(TileSourceViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> Get(Guid tileSourceId)
		{
			TileSource tileSource = await TileSourceRepository.GetTileSourceWithId(tileSourceId);

			if (tileSource == null)
			{
				return NotFound();
			}

			return Ok(Mapper.Map<TileSourceViewModel>(tileSource));
		}

		[HttpPost]
		[ProducesResponseType(typeof(TileSourceViewModel), (int)HttpStatusCode.Created)]
		public async Task<IActionResult> Post([FromBody] CreateTileSourceViewModel viewModel)
		{
			TileSource tileSource = await TileSourceRepository.CreateTileSource(Mapper.Map<TileSource>(viewModel));

			return CreatedAtAction(nameof(Get), new
			{
				tileSourceId = tileSource.TileSourceId,
			}, Mapper.Map<TileSourceViewModel>(tileSource));
		}
	}
}