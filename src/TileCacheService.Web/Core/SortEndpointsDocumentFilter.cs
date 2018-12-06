// <copyright file="SortEndpointsDocumentFilter.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core
{
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.Swagger;
	using Swashbuckle.AspNetCore.SwaggerGen;

	public class SortEndpointsDocumentFilter : IDocumentFilter
	{
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			List<KeyValuePair<string, OpenApiPathItem>> paths = swaggerDoc.Paths.OrderBy(e => e.Key).ToList();

			swaggerDoc.Paths = new OpenApiPaths();
			foreach (KeyValuePair<string, OpenApiPathItem> item in paths.ToDictionary(e => e.Key, e => e.Value))
			{
				swaggerDoc.Paths.Add(item.Key, item.Value);
			}
		}
	}
}