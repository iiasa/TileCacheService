// <copyright file="EntityToViewModelProfile.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core.Mapping
{
	using System.Linq;
	using AutoMapper;
	using TileCacheService.Data.Entities;
	using TileCacheService.Shared.Enums;
	using TileCacheService.Web.Models;

	public class EntityToViewModelProfile : Profile
	{
		public EntityToViewModelProfile()
		{
			CreateMap<TileSource, TileSourceViewModel>()
				.ForMember(dest => dest.TileServerUrls, opt => opt.MapFrom(src => src.TileServerUrls.Select(x => x.Url)));
			CreateMap<TileSource, CreateTileSourceViewModel>()
				.ForMember(dest => dest.TileServerUrls, opt => opt.MapFrom(src => src.TileServerUrls.Select(x => x.Url)));

			CreateMap<TileCache, TileCacheViewModel>()
				.ForMember(dest => dest.Status,
					opt => opt.MapFrom(src =>
						src.ProcessingStarted.HasValue
							? (src.ProcessingFinished.HasValue ? TileCacheStatusEnum.Finished : TileCacheStatusEnum.Processing)
							: TileCacheStatusEnum.New));
			CreateMap<TileCache, CreateTileCacheViewModel>();
		}
	}
}