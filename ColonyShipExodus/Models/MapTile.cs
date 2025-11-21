using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

public class MapTile
{
    public BiomeType Biome { get; set; }
    public bool IsExplored { get; set; }
    public Dictionary<ResourceType, int> Resources { get; } = new();

    public MapTile(BiomeType biome)
    {
        Biome = biome;
        IsExplored = false;
    }
}