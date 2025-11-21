using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

public class Building
{
    public BuildingType Type { get; }
    public int BuildProgress { get; set; }
    public int RequiredProgress { get; }
    public bool IsComplete => BuildProgress >= RequiredProgress;

    public Building(BuildingType type)
    {
        Type = type;
        BuildProgress = 0;
        RequiredProgress = type switch
        {
            BuildingType.Shelter => 50,
            BuildingType.Farm => 100,
            BuildingType.WaterPurifier => 75,
            BuildingType.Infirmary => 120,
            _ => 100
        };
    }
}