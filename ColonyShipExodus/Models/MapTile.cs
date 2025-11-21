namespace ColonyShipExodus.Models
{
    public enum BiomeType
    {
        Forest,
        Mine,
        Ruins,
        Plains,
        Mountain
    }

    public class MapTile
    {
        public BiomeType Biome { get; set; }
        public int Resources { get; set; } // Generic resource value
        public bool IsExplored { get; set; }

        public MapTile(BiomeType biome)
        {
            Biome = biome;
            Resources = GetStartingResources(biome);
            IsExplored = false;
        }

        private int GetStartingResources(BiomeType biome)
        {
            switch (biome)
            {
                case BiomeType.Forest:
                    return 50; // Example: Wood
                case BiomeType.Mine:
                    return 75; // Example: Ore
                case BiomeType.Ruins:
                    return 25; // Example: Scrap
                case BiomeType.Plains:
                    return 30;
                case BiomeType.Mountain:
                    return 60;
                default:
                    return 0;
            }
        }
    }
}