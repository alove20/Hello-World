using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Services;

public class WorldGenerator
{
    private readonly Random _random = new();

    public Map GenerateMap(int width, int height)
    {
        var map = new Map(width, height);
        var crashSiteX = width / 2;
        var crashSiteY = height / 2;
        map.CrashSiteLocation = (crashSiteX, crashSiteY);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                BiomeType biome;
                if (x == crashSiteX && y == crashSiteY)
                {
                    biome = BiomeType.CrashSite;
                }
                else
                {
                    // Simple procedural generation based on distance from crash site
                    var distance = Math.Sqrt(Math.Pow(x - crashSiteX, 2) + Math.Pow(y - crashSiteY, 2));
                    if (distance < 2)
                    {
                        biome = BiomeType.Plains;
                    }
                    else if (distance < 4)
                    {
                        biome = (BiomeType)_random.Next(1, 4); // Forest, Mountains, Plains
                    }
                    else
                    {
                        biome = (BiomeType)_random.Next(4, 6); // Ruins, Wasteland
                    }
                }
                map.Grid[x, y] = new MapTile(biome);
                PopulateTileResources(map.Grid[x, y]);
            }
        }
        
        map.Grid[crashSiteX, crashSiteY].IsExplored = true;
        
        return map;
    }

    private void PopulateTileResources(MapTile tile)
    {
        switch (tile.Biome)
        {
            case BiomeType.Forest:
                tile.Resources.Add(ResourceType.Food, _random.Next(10, 31));
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(20, 51));
                break;
            case BiomeType.Mountains:
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(30, 61));
                tile.Resources.Add(ResourceType.Energy, _random.Next(5, 21)); // Raw energy source
                break;
            case BiomeType.Ruins:
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(10, 41));
                tile.Resources.Add(ResourceType.Medicine, _random.Next(0, 11));
                break;
            case BiomeType.Plains:
                tile.Resources.Add(ResourceType.Food, _random.Next(5, 21));
                break;
            case BiomeType.Wasteland:
                // Sparse resources
                break;
            case BiomeType.CrashSite:
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(5, 15));
                break;
        }
    }
}