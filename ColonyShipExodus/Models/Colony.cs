namespace ColonyShipExodus.Models;

/// <summary>
/// Manages the colony's state, including survivors, resources, and buildings.
/// </summary>
public class Colony
{
    public List<Survivor> Survivors { get; } = new();
    public Dictionary<ResourceType, int> Resources { get; } = new();
    public List<BuildingType> Buildings { get; } = new();

    public Colony()
    {
        // Initialize resources
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = 0;
        }
        Resources[ResourceType.Food] = 50;
        Resources[ResourceType.Water] = 50;
        Resources[ResourceType.BuildingMaterials] = 20;

        // Initialize starting survivors
        Survivors.Add(new Survivor("Elena (Commander)", SurvivorSkill.None));
        Survivors.Add(new Survivor("Dr. Aris", SurvivorSkill.Doctor));
        Survivors.Add(new Survivor("Jax", SurvivorSkill.Engineer));
        Survivors.Add(new Survivor("Kara", SurvivorSkill.Scavenger));
        Survivors.Add(new Survivor("Leo", SurvivorSkill.Farmer));
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (amount > 0)
        {
            Resources[type] += amount;
        }
    }

    public bool UseResource(ResourceType type, int amount)
    {
        if (Resources.ContainsKey(type) && Resources[type] >= amount)
        {
            Resources[type] -= amount;
            return true;
        }
        return false;
    }
}