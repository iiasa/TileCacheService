// <copyright file="Startup.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web
{
	using System;
	using System.IO;
	using System.Linq;
	using AutoMapper;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Diagnostics.HealthChecks;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.OpenApi.Models;
	using Newtonsoft.Json;
	using Swashbuckle.AspNetCore.Swagger;
	using TileCacheService.Data;
	using TileCacheService.Data.Repositories;
	using TileCacheService.Web.Core;
	using TileCacheService.Web.Core.HealthChecks;

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

			HealthCheckOptions options = new HealthCheckOptions
			{
				ResponseWriter = async (c, r) =>
				{
					string result = JsonConvert.SerializeObject(new
					{
						status = r.Status.ToString(),
						checks = r.Entries.Select(e => new
						{
							key = e.Key,
							value = e.Value.Status.ToString(),
						}),
					});

					c.Response.ContentType = "application/json";
					await c.Response.WriteAsync(result);
				},
			};
			app.UseHealthChecks("/health", options);

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

			services.AddHealthChecks().AddCheck<DbContextHealthCheck>("DbContext").AddCheck<FilesystemHealthCheck>("FileSystem");

			services.AddSwaggerGen(c =>
			{
				c.DescribeAllEnumsAsStrings();
				c.DocumentFilter<SortEndpointsDocumentFilter>();
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "TileCache API",
					Version = "v1",
					Description = "Generate tile caches based on manageable data sources.",
					TermsOfService = null,
					Contact = new OpenApiContact
					{
						Name = "Christoph Perger",
						Email = string.Empty,
						Url = new Uri("https://github.com/pergerch"),
					},
					License = new OpenApiLicense
					{
						Name = "MIT License",
						Url = new Uri("https://opensource.org/licenses/MIT"),
					},
				});

				string filePath = Path.Combine(System.AppContext.BaseDirectory, "TileCacheAPI.xml");
				c.IncludeXmlComments(filePath);
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}
	}
}