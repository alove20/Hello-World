namespace ColonyShipExodus.Models
{
    public class Building
    {
        public BuildingType Type { get; set; }
        public BuildingStatus Status { get; set; }
        public int ConstructionProgress { get; set; } // from 0 to 100

        public Building(BuildingType type)
        {
            Type = type;
            Status = BuildingStatus.UnderConstruction;
            ConstructionProgress = 0;
        }
    }
}