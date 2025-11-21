using System.Collections.Generic;

namespace ColonyShipExodus.Models
{
    public class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public BiomeType Biome { get; set; }
        public Dictionary<ResourceType, int> ResourceDeposits { get; set; }
        public bool Explored { get; set; }

        public MapTile(int x, int y, BiomeType biome)
        {
            X = x;
            Y = y;
            Biome = biome;
            Explored = false;
            ResourceDeposits = new Dictionary<ResourceType, int>();
        }
    }
}