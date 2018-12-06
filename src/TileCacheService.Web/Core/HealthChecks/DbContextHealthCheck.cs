// <copyright file="DbContextHealthCheck.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core.HealthChecks
{
	using System;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Diagnostics.HealthChecks;
	using TileCacheService.Data;

	public class DbContextHealthCheck : IHealthCheck
	{
		public DbContextHealthCheck(TileCacheServiceContext context)
		{
			Context = context;
		}

		private TileCacheServiceContext Context { get; }

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				int i = Context.TileSources.Count();

				return Task.FromResult(HealthCheckResult.Healthy($"{i} TileSources"));
			}
			catch (Exception)
			{
				return Task.FromResult(HealthCheckResult.Unhealthy());
			}
		}
	}
}