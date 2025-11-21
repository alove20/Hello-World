using System.Collections.Generic;

namespace ColonyShipExodus.Models
{
    public class Building
    {
        public string Name { get; set; }
        public BuildingType Type { get; set; }
        public bool IsCompleted { get; set; }
        public Dictionary<ResourceType, int> Cost { get; set; }
        public int ConstructionProgress { get; set; }
        public int RequiredProgress { get; set; }
        public string Description { get; set; }

        public Building(string name, BuildingType type, Dictionary<ResourceType, int> cost, int requiredProgress, string desc)
        {
            Name = name;
            Type = type;
            Cost = new Dictionary<ResourceType, int>(cost);
            RequiredProgress = requiredProgress;
            Description = desc;
            IsCompleted = false;
            ConstructionProgress = 0;
        }
    }

    public static class BuildingCatalog
    {
        public static Dictionary<BuildingType, Building> Definitions = new Dictionary<BuildingType, Building>()
        {
            [BuildingType.Shelter] = new Building(
                "Shelter",
                BuildingType.Shelter,
                new Dictionary<ResourceType, int> {{ResourceType.BuildingMaterials, 12 }, {ResourceType.Energy, 2}},
                4,
                "Provides safe rest and small morale increase."
            ),
            [BuildingType.Farm] = new Building(
                "Farm",
                BuildingType.Farm,
                new Dictionary<ResourceType, int> {{ResourceType.BuildingMaterials, 8 }, {ResourceType.Water, 4}},
                4,
                "Increases daily food production."
            ),
            [BuildingType.WaterPurifier] = new Building(
                "Water Purifier",
                BuildingType.WaterPurifier,
                new Dictionary<ResourceType, int> {{ResourceType.BuildingMaterials, 10 }, {ResourceType.Energy, 2}},
                4,
                "Increases daily water production."
            ),
            [BuildingType.Infirmary] = new Building(
                "Infirmary",
                BuildingType.Infirmary,
                new Dictionary<ResourceType, int> {{ResourceType.BuildingMaterials, 9 }, {ResourceType.Medicine, 3}},
                4,
                "Improves healing of sick/injured survivors."
            ),
            [BuildingType.PowerGenerator] = new Building(
                "Power Generator",
                BuildingType.PowerGenerator,
                new Dictionary<ResourceType, int> {{ResourceType.BuildingMaterials, 14 }},
                4,
                "Increases daily energy production."
            ),
        };

        public static Building GetDefinition(BuildingType type)
        {
            var def = Definitions[type];
            // Return a copy
            return new Building(def.Name, def.Type, def.Cost, def.RequiredProgress, def.Description);
        }

        public static Building CreateBuilding(BuildingType type)
        {
            return GetDefinition(type);
        }

        public static List<BuildingType> GetAvailableBuildings(List<Building> current, Inventory inventory)
        {
            var result = new List<BuildingType>();
            foreach (var kvp in Definitions)
            {
                // Already completed or under construction?
                if (current.Exists(b => b.Type == kvp.Key && !b.IsCompleted))
                    continue;
                var bld = kvp.Value;
                bool canBuild = true;
                foreach (var (rtype, amt) in bld.Cost)
                {
                    if (!inventory.HasEnough(rtype, amt))
                    {
                        canBuild = false;
                        break;
                    }
                }
                if (canBuild)
                    result.Add(kvp.Key);
            }
            return result;
        }

        public static string CostString(this Building b)
        {
            List<string> parts = new List<string>();
            foreach(var (rtype, amt) in b.Cost)
            {
                parts.Add($"{amt} {rtype}");
            }
            return string.Join(", ", parts);
        }
    }
}