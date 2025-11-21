namespace ColonyShipExodus.World;

/// <summary>
/// Represents a single tile on the world map.
/// </summary>
public class MapTile
{
    public BiomeType Biome { get; set; }
    public bool IsExplored { get; set; }
    // Future expansion: resources available on this tile
    // public Dictionary<ResourceType, int> Resources { get; set; }

    public MapTile(BiomeType biome)
    {
        Biome = biome;
        IsExplored = false;
    }
}