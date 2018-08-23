// <copyright file="ViewModelToEntityProfile.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core.Mapping
{
	using AutoMapper;
	using TileCacheService.Data.Entities;
	using TileCacheService.Web.Models;

	public class ViewModelToEntityProfile : Profile
	{
		public ViewModelToEntityProfile()
		{
			CreateMap<string, TileServerUrl>()
				.ForMember(dest => dest.TileSourceId, opt => opt.Ignore())
				.ForMember(dest => dest.TileSource, opt => opt.Ignore())
				.ForMember(dest => dest.TileServerUrlId, opt => opt.Ignore())
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src));

			CreateMap<TileSourceViewModel, TileSource>();
			CreateMap<CreateTileSourceViewModel, TileSource>()
				.ForMember(dest => dest.TileSourceId, opt => opt.Ignore())
				.ForMember(dest => dest.TileServerUrls, opt => opt.MapFrom(src => src.TileServerUrls));

			CreateMap<TileCacheViewModel, TileCache>()
				.ForMember(dest => dest.TileSource, opt => opt.Ignore())
				.ForMember(dest => dest.Filename, opt => opt.Ignore())
				.ForMember(dest => dest.RetryCount, opt => opt.Ignore())
				.ForMember(dest => dest.ProcessingError, opt => opt.Ignore())
				.ForMember(dest => dest.ProcessingStarted, opt => opt.Ignore())
				.ForMember(dest => dest.ProcessingFinished, opt => opt.Ignore());
			CreateMap<CreateTileCacheViewModel, TileCache>()
				.ForMember(dest => dest.TileCacheId, opt => opt.Ignore())
				.ForMember(dest => dest.TileSource, opt => opt.Ignore())
				.ForMember(dest => dest.Filename, opt => opt.Ignore())
				.ForMember(dest => dest.RetryCount, opt => opt.Ignore())
				.ForMember(dest => dest.ProcessingError, opt => opt.Ignore())
				.ForMember(dest => dest.ProcessingStarted, opt => opt.Ignore())
				.ForMember(dest => dest.ProcessingFinished, opt => opt.Ignore());
		}
	}
}