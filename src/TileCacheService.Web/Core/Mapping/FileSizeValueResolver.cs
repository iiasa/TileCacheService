// <copyright file="FileSizeValueResolver.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core.Mapping
{
	using System.IO;
	using AutoMapper;
	using Microsoft.Extensions.Options;
	using TileCacheService.Data.Entities;
	using TileCacheService.Web.Models;

	public class FileSizeValueResolver : IValueResolver<TileCache, TileCacheViewModel, long>
	{
		public FileSizeValueResolver(IOptions<ServiceOptions> serviceOptions)
		{
			ServiceOptions = serviceOptions.Value;
		}

		public ServiceOptions ServiceOptions { get; }

		public long Resolve(TileCache source, TileCacheViewModel destination, long destMember, ResolutionContext context)
		{
			return new FileInfo(Path.Combine(ServiceOptions.DirectoryRoot, "TileCaches", source.Filename)).Length;
		}
	}
}