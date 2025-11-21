using ColonyShipExodus.Enums;
using System.Collections.Generic;

namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a building in the colony.
/// </summary>
public class Building
{
    public BuildingType Type { get; }
    public bool IsComplete { get; set; }
    public int WorkRequired { get; }
    public int WorkApplied { get; set; }

    public Building(BuildingType type)
    {
        Type = type;
        IsComplete = false;
        WorkApplied = 0;
        WorkRequired = GetBuildingWorkCost(type);
    }

    /// <summary>
    /// Gets the resource cost to start building a structure.
    /// </summary>
    public static Dictionary<ResourceType, int> GetResourceCost(BuildingType type)
    {
        return type switch
        {
            BuildingType.Shelter => new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 50 } },
            BuildingType.Farm => new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 30 }, { ResourceType.Food, 10 } },
            BuildingType.WaterPurifier => new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 40 }, { ResourceType.Energy, 5 } },
            BuildingType.Infirmary => new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 60 }, { ResourceType.Medicine, 10 } },
            _ => new Dictionary<ResourceType, int>()
        };
    }

    /// <summary>
    /// Gets the work (man-days) required to complete a building.
    /// </summary>
    private static int GetBuildingWorkCost(BuildingType type)
    {
        return type switch
        {
            BuildingType.Shelter => 20,
            BuildingType.Farm => 15,
            BuildingType.WaterPurifier => 25,
            BuildingType.Infirmary => 30,
            _ => 0
        };
    }
}