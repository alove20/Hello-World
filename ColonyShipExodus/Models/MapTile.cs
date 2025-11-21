namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single tile on the game world map.
/// </summary>
public class MapTile
{
    public BiomeType Biome { get; set; }

    public MapTile(BiomeType biome)
    {
        Biome = biome;
    }

    public override string ToString()
    {
        return $"[{Biome}]";
    }
}