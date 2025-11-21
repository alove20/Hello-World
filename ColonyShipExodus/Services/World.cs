using ColonyShipExodus.Models;

namespace ColonyShipExodus.Services;

/// <summary>
/// Manages the game map and procedural generation.
/// </summary>
public class World
{
    public MapTile[,] Grid { get; }
    private readonly int _width;
    private readonly int _height;
    private readonly Random _random = new();

    public World(int width, int height)
    {
        _width = width;
        _height = height;
        Grid = new MapTile[width, height];
    }

    public void GenerateMap()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Grid[x, y] = new MapTile(GetRandomBiome());
            }
        }
        // Set the center as the crash site
        Grid[_width / 2, _height / 2] = new MapTile(BiomeType.CrashSite);
    }

    private BiomeType GetRandomBiome()
    {
        int value = _random.Next(100);
        if (value < 40) return BiomeType.Plains;
        if (value < 70) return BiomeType.Forest;
        if (value < 90) return BiomeType.Mountains;
        return BiomeType.Ruins;
    }
}