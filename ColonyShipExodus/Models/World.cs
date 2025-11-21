using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

/// <summary>
/// Represents the game world map.
/// </summary>
public class World
{
    public MapTile[,] Map { get; }
    public int Width => Map.GetLength(0);
    public int Height => Map.GetLength(1);

    public World(int width, int height)
    {
        Map = new MapTile[width, height];
        GenerateMap();
    }

    /// <summary>
    /// Procedurally generates the map with various biomes.
    /// </summary>
    private void GenerateMap()
    {
        Random rand = new();
        var biomeTypes = Enum.GetValues(typeof(BiomeType)).Cast<BiomeType>().Where(b => b != BiomeType.CrashSite).ToArray();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                var randomBiome = biomeTypes[rand.Next(biomeTypes.Length)];
                Map[x, y] = new MapTile(randomBiome);
            }
        }

        // Place the crash site at the center
        int centerX = Width / 2;
        int centerY = Height / 2;
        Map[centerX, centerY] = new MapTile(BiomeType.CrashSite);
        Map[centerX, centerY].IsExplored = true;
    }
}