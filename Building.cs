namespace ColonyShipExodus
{
    public class Building
    {
        public string Name { get; set; }
        public BuildingType Type { get; set; }
        public int Level { get; set; }

        public Building(string name, BuildingType type)
        {
            Name = name;
            Type = type;
            Level = 1;
        }
    }

    public enum BuildingType
    {
        Shelter,
        Farm,
        WaterPurifier,
        Infirmary
    }
}