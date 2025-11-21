namespace ColonyShipExodus.Models
{
    public enum BuildingType
    {
        Shelter,
        Farm,
        WaterPurifier,
        Infirmary
    }

    public class Building
    {
        public BuildingType Type { get; set; }
        public bool IsBuilt { get; set; }
        public int BuildProgress { get; set; } // 0-100

        public Building(BuildingType type)
        {
            Type = type;
            IsBuilt = false;
            BuildProgress = 0;
        }
    }
}