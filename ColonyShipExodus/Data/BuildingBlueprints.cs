using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Data;

public static class BuildingBlueprints
{
    public static readonly Dictionary<BuildingType, Building> Blueprints = new()
    {
        {
            BuildingType.Shelter, new Building(
                BuildingType.Shelter,
                requiredProgress: 20,
                cost: new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 15 } }
            )
        },
        {
            BuildingType.Farm, new Building(
                BuildingType.Farm,
                requiredProgress: 30,
                cost: new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 25 }, { ResourceType.Water, 10 } }
            )
        },
        {
            BuildingType.WaterPurifier, new Building(
                BuildingType.WaterPurifier,
                requiredProgress: 25,
                cost: new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 20 } }
            )
        },
        {
            BuildingType.Infirmary, new Building(
                BuildingType.Infirmary,
                requiredProgress: 40,
                cost: new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 30 }, { ResourceType.Medicine, 10 } }
            )
        },
        {
            BuildingType.Workshop, new Building(
                BuildingType.Workshop,
                requiredProgress: 50,
                cost: new Dictionary<ResourceType, int> { { ResourceType.BuildingMaterials, 40 }, { ResourceType.Energy, 5 } }
            )
        }
    };

    public static Building CreateBuilding(BuildingType type)
    {
        if (Blueprints.TryGetValue(type, out var blueprint))
        {
            // Create a new instance from the blueprint
            return new Building(blueprint.Type, blueprint.RequiredProgress, new Dictionary<ResourceType, int>(blueprint.Cost));
        }
        throw new ArgumentException($"No blueprint found for building type: {type}");
    }
}