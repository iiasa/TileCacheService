// <copyright file="Program.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web
{
	using System;
	using System.IO;
	using Microsoft.AspNetCore;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Serilog;

	public class Program
	{
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();

		public static void Main(string[] args)
		{
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

			IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false, true)
				.AddJsonFile($"appsettings.{environment}.json", true)
				.Build();

			CreateWebHostBuilder(args)
				.UseConfiguration(configuration)
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddSerilog(new LoggerConfiguration().ReadFrom
						.Configuration(hostingContext.Configuration.GetSection("Logging"))
						.CreateLogger());
				})
				.Build()
				.Run();
		}
	}
}