// <copyright file="ITileCacheManager.cs" company="IIASA">
// Copyright (c) IIASA. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace TileCacheService.Shared.Services
{
	using TileCacheService.Shared.Entities;

	public interface ITileCacheManager
	{
		void ClearTiles();

		int GetTilesCount();

		void SaveTile(int zoomLevel, int tileRow, int tileColumn, byte[] tileData);

		Tile TryGetTile(int tileColumn, int tileRow, int zoomLevel);
	}
}