using System.Text;

namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single tile on the game world map.
/// </summary>
public class MapTile
{
    public BiomeType Biome { get; }

    public MapTile(BiomeType biome)
    {
        Biome = biome;
    }

    /// <summary>
    /// Gets a character representation for the map display.
    /// </summary>
    public char GetMapChar()
    {
        return Biome switch
        {
            BiomeType.Forest => 'F',
            BiomeType.Plains => '.',
            BiomeType.Mountains => '^',
            BiomeType.Ruins => 'R',
            BiomeType.CrashSite => 'X',
            _ => '?'
        };
    }

    /// <summary>
    /// Gets the console color for the map display.
    /// </summary>
    public ConsoleColor GetMapColor()
    {
        return Biome switch
        {
            BiomeType.Forest => ConsoleColor.DarkGreen,
            BiomeType.Plains => ConsoleColor.Green,
            BiomeType.Mountains => ConsoleColor.Gray,
            BiomeType.Ruins => ConsoleColor.DarkYellow,
            BiomeType.CrashSite => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }
}