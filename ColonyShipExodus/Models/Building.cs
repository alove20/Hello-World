using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

public class Building
{
    public BuildingType Type { get; }
    public int Progress { get; set; } // Progress towards completion (e.g., man-days)
    public int RequiredProgress { get; }
    public bool IsCompleted => Progress >= RequiredProgress;
    public Dictionary<ResourceType, int> Cost { get; }

    public Building(BuildingType type, int requiredProgress, Dictionary<ResourceType, int> cost)
    {
        Type = type;
        RequiredProgress = requiredProgress;
        Cost = cost;
    }

    public void AddProgress(int amount)
    {
        if (!IsCompleted)
        {
            Progress = Math.Min(Progress + amount, RequiredProgress);
        }
    }
}