using ColonyShipExodus.Models;

namespace ColonyShipExodus.Models
{
    public class MapTile
    {
        public BiomeType BiomeType { get; set; }
        public bool IsExplored { get; set; }
        public string SpecialDescription { get; set; }

        public MapTile(BiomeType type, bool isExplored = false)
        {
            BiomeType = type;
            IsExplored = isExplored;
            SpecialDescription = null;
        }
    }
}