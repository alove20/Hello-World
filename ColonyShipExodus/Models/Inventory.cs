namespace ColonyShipExodus.Models;

/// <summary>
/// Manages the colony's resources.
/// </summary>
public class Inventory
{
    private readonly Dictionary<ResourceType, int> _resources;

    public Inventory()
    {
        _resources = new Dictionary<ResourceType, int>();
        // Initialize all resource types to 0
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            _resources[type] = 0;
        }
    }

    public int GetResourceCount(ResourceType type)
    {
        return _resources.GetValueOrDefault(type, 0);
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (amount > 0)
        {
            _resources[type] += amount;
        }
    }

    public bool UseResource(ResourceType type, int amount)
    {
        if (GetResourceCount(type) >= amount)
        {
            _resources[type] -= amount;
            return true;
        }

        _resources[type] = 0; // Can't go below zero
        return false;
    }

    public void Display()
    {
        Console.WriteLine($"  Food: {GetResourceCount(ResourceType.Food)} | Water: {GetResourceCount(ResourceType.Water)} | Materials: {GetResourceCount(ResourceType.BuildingMaterials)} | Medicine: {GetResourceCount(ResourceType.Medicine)}");
    }
}