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
		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;
			Environment = env;
		}

		public IConfiguration Configuration { get; }

		public IHostingEnvironment Environment { get; }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			mapper.ConfigurationProvider.AssertConfigurationIsValid();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "TileCache API V1");
				c.RoutePrefix = "docs";
			});

			app.UseMvc();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			string rootDir = Configuration.GetValue<string>("RootDir");
			if (string.IsNullOrEmpty(rootDir))
			{
				rootDir = Environment.WebRootPath;
			}

			services.Configure<ServiceOptions>(options => { options.DirectoryRoot = rootDir; });

			string dbPath = Path.Combine(rootDir, "tile-cache-service.db");
			services.AddDbContext<TileCacheServiceContext>(options => options.UseSqlite($"Data Source={dbPath}").UseLazyLoadingProxies());

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