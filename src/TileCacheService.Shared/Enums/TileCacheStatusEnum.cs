// <copyright file="TileCacheStatusEnum.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared.Enums
{
	public enum TileCacheStatusEnum
	{
		New = 1,

		Processing,

		Finished,

		Error,
	}
}