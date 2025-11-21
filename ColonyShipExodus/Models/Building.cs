namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a structure that can be built in the colony.
/// </summary>
public class Building
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Dictionary<ResourceType, int> Cost { get; set; }

    public Building(string name, string description, Dictionary<ResourceType, int> cost)
    {
        Name = name;
        Description = description;
        Cost = cost;
    }
}