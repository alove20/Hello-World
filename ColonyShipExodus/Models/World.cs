namespace ColonyShipExodus.Models;

/// <summary>
/// Represents and generates the game world map.
/// </summary>
public class World
{
    public MapTile[,] Grid { get; }
    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);
    public (int X, int Y) CrashSiteLocation { get; private set; }

    private readonly Random _random = new();

    public World(int width, int height)
    {
        Grid = new MapTile[width, height];
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                // Simple procedural generation
                var val = _random.NextDouble();
                BiomeType biome;
                if (val < 0.4) biome = BiomeType.Forest;
                else if (val < 0.7) biome = BiomeType.Plains;
                else if (val < 0.9) biome = BiomeType.Mountains;
                else biome = BiomeType.Ruins;
                Grid[x, y] = new MapTile(biome);
            }
        }

        // Place the crash site in the center
        CrashSiteLocation = (Width / 2, Height / 2);
        Grid[CrashSiteLocation.X, CrashSiteLocation.Y] = new MapTile(BiomeType.CrashSite);
    }

    public void DisplayMap()
    {
        Console.WriteLine("\n--- World Map ---");
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Grid[x, y];
                Console.ForegroundColor = tile.GetMapColor();
                Console.Write(tile.GetMapChar() + " ");
            }
            Console.WriteLine();
        }
        Console.ResetColor();
        Console.WriteLine("X: Crash Site, F: Forest, .: Plains, ^: Mountains, R: Ruins");
    }
}