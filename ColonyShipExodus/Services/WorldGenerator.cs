using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Services;

public class WorldGenerator
{
    private readonly Random _random = new();

    public MapTile[,] GenerateMap(int width, int height)
    {
        var map = new MapTile[width, height];
        var biomeTypes = Enum.GetValues(typeof(BiomeType)).Cast<BiomeType>()
            .Where(b => b != BiomeType.CrashSite).ToArray();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var biome = biomeTypes[_random.Next(biomeTypes.Length)];
                map[x, y] = new MapTile(biome);
                PopulateTileResources(map[x, y]);
            }
        }

        // Set the crash site in the middle
        int centerX = width / 2;
        int centerY = height / 2;
        map[centerX, centerY] = new MapTile(BiomeType.CrashSite) { IsExplored = true };
        PopulateTileResources(map[centerX, centerY]);

        return map;
    }

    private void PopulateTileResources(MapTile tile)
    {
        switch (tile.Biome)
        {
            case BiomeType.CrashSite:
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(50, 101));
                tile.Resources.Add(ResourceType.Food, _random.Next(5, 15));
                break;
            case BiomeType.Forest:
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(20, 51));
                tile.Resources.Add(ResourceType.Food, _random.Next(10, 31));
                break;
            case BiomeType.Mountains:
                tile.Resources.Add(ResourceType.BuildingMaterials, _random.Next(30, 81));
                break;
            case BiomeType.Ruins:
                tile.Resources.Add(ResourceType.Energy, _random.Next(5, 26));
                tile.Resources.Add(ResourceType.Medicine, _random.Next(0, 11));
                break;
            case BiomeType.Plains:
                tile.Resources.Add(ResourceType.Food, _random.Next(5, 21));
                break;
        }
    }
}