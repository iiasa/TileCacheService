// <copyright file="Startup.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web
{
	using System.IO;
	using AutoMapper;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Swashbuckle.AspNetCore.Swagger;
	using TileCacheService.Data;
	using TileCacheService.Data.Repositories;
	using TileCacheService.Web.Core;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			mapper.ConfigurationProvider.AssertConfigurationIsValid();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "TileCache API V1");
				c.RoutePrefix = "docs";
			});

			app.UseHttpsRedirection();
			app.UseMvc();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<TileCacheServiceContext>(options =>
				options.UseSqlite("Data Source=" + Path.Combine("Data", "tile-cache-service.db")));

			services.AddTransient<TileCacheRepository>();
			services.AddTransient<TileSourceRepository>();

			services.AddAutoMapper();

			services.AddHostedService<OfflineCacheBackgroundTask>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSwaggerGen(c =>
			{
				c.DescribeAllEnumsAsStrings();
				c.DocumentFilter<SortEndpointsDocumentFilter>();
				c.SwaggerDoc("v1", new Info
				{
					Title = "TileCache API",
					Version = "v1",
					Description = "Generate tile caches based on manageable data sources.",
					TermsOfService = "None",
					Contact = new Contact
					{
						Name = "Christoph Perger",
						Email = string.Empty,
						Url = "https://github.com/pergerch",
					},
					License = new License
					{
						Name = "MIT License",
						Url = "https://opensource.org/licenses/MIT",
					},
				});
			});
		}
	}
}