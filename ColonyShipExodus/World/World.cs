namespace ColonyShipExodus.World;

/// <summary>
/// Represents the game world map.
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
        GenerateMap();
    }

    /// <summary>
    /// Procedurally generates the map with various biomes.
    /// </summary>
    private void GenerateMap()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Grid[x, y] = new MapTile(GetRandomBiome());
            }
        }

        // Ensure the center is the crash site
        int centerX = _width / 2;
        int centerY = _height / 2;
        Grid[centerX, centerY] = new MapTile(BiomeType.CrashSite);
    }

    private BiomeType GetRandomBiome()
    {
        int roll = _random.Next(100);
        if (roll < 40) return BiomeType.Plains;      // 40% chance
        if (roll < 70) return BiomeType.Forest;      // 30% chance
        if (roll < 90) return BiomeType.Mountains;   // 20% chance
        return BiomeType.Ruins;                      // 10% chance
    }
}