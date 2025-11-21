using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single tile on the game map.
/// </summary>
public class MapTile
{
    public BiomeType Biome { get; }
    public bool IsExplored { get; set; }

    public MapTile(BiomeType biome)
    {
        Biome = biome;
        IsExplored = false;
    }
}