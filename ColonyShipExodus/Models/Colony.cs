using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

/// <summary>
/// Represents the player's colony, managing survivors, resources, and buildings.
/// </summary>
public class Colony
{
    public List<Survivor> Survivors { get; } = new();
    public Dictionary<ResourceType, int> Resources { get; } = new();
    public List<Building> Buildings { get; } = new();
    public Building? InProgressBuilding { get; set; }

    public Colony()
    {
        // Initialize starting resources
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = 0;
        }
        Resources[ResourceType.Food] = 50;
        Resources[ResourceType.Water] = 50;
        Resources[ResourceType.BuildingMaterials] = 20;

        // Initialize starting survivors
        Survivors.Add(new Survivor("Jian Li", 100, 80));
        Survivors.Add(new Survivor("Elena Petrova", 100, 80));
        Survivors.Add(new Survivor("Marcus Cole", 100, 80));
        Survivors.Add(new Survivor("Aisha Sharma", 100, 80));
    }
    
    /// <summary>
    /// Modifies the amount of a resource.
    /// </summary>
    public void AddResource(ResourceType type, int amount)
    {
        if (Resources.ContainsKey(type))
        {
            Resources[type] += amount;
            if (Resources[type] < 0) Resources[type] = 0;
        }
    }
}