// <copyright file="SortEndpointsDocumentFilter.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using Swashbuckle.AspNetCore.Swagger;
	using Swashbuckle.AspNetCore.SwaggerGen;

	public class SortEndpointsDocumentFilter : IDocumentFilter
	{
		public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
		{
			List<KeyValuePair<string, PathItem>> paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();

			swaggerDoc.Paths = paths.ToDictionary(e => e.Key, e => e.Value);
		}
	}
}