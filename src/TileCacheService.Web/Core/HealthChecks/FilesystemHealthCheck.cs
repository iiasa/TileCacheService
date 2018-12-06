// <copyright file="FilesystemHealthCheck.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Web.Core.HealthChecks
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Diagnostics.HealthChecks;
	using Microsoft.Extensions.Options;

	public class FilesystemHealthCheck : IHealthCheck
	{
		public FilesystemHealthCheck(IOptions<ServiceOptions> serviceOptions)
		{
			ServiceOptions = serviceOptions.Value;
		}

		private ServiceOptions ServiceOptions { get; }

		public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(ServiceOptions.DirectoryRoot);

				if (directoryInfo.Exists)
				{
					return Task.FromResult(HealthCheckResult.Healthy());
				}

				return Task.FromResult(HealthCheckResult.Unhealthy());
			}
			catch (Exception)
			{
				return Task.FromResult(HealthCheckResult.Unhealthy());
			}
		}
	}
}