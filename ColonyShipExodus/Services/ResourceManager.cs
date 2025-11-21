using ColonyShipExodus.Models;
using System.Text;

namespace ColonyShipExodus.Services;

/// <summary>
/// Manages the colony's inventory of resources.
/// </summary>
public class ResourceManager
{
    private readonly Dictionary<ResourceType, int> _resources;

    public ResourceManager()
    {
        _resources = new Dictionary<ResourceType, int>();
        // Initialize all resource types to 0
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            _resources[type] = 0;
        }
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
        if (_resources.ContainsKey(type) && _resources[type] >= amount)
        {
            _resources[type] -= amount;
            return true;
        }
        // If not enough, use what's left
        _resources[type] = 0;
        return false;
    }

    public bool HasEnoughResources(Dictionary<ResourceType, int> costs)
    {
        return costs.All(cost => _resources.ContainsKey(cost.Key) && _resources[cost.Key] >= cost.Value);
    }
    
    public void UseResources(Dictionary<ResourceType, int> costs)
    {
        foreach (var cost in costs)
        {
            UseResource(cost.Key, cost.Value);
        }
    }

    public int GetResourceCount(ResourceType type)
    {
        return _resources.GetValueOrDefault(type, 0);
    }

    public void DisplayResources()
    {
        var sb = new StringBuilder();
        foreach (var resource in _resources)
        {
            sb.Append($"{resource.Key}: {resource.Value} | ");
        }
        Console.WriteLine(sb.ToString().TrimEnd(' ', '|'));
    }
}