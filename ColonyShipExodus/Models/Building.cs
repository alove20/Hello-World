namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a structure that can be built by the colony.
/// </summary>
public class Building
{
    public BuildingType Type { get; }
    public int Progress { get; private set; }
    public int RequiredProgress { get; }
    public bool IsComplete => Progress >= RequiredProgress;

    public Building(BuildingType type)
    {
        Type = type;
        Progress = 0;
        RequiredProgress = GetRequiredProgressForType(type);
    }

    public void AddProgress(int amount)
    {
        if (!IsComplete)
        {
            Progress += amount;
            if (IsComplete)
            {
                Progress = RequiredProgress; // Cap progress
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Construction of {Type} is complete!");
                Console.ResetColor();
            }
        }
    }
    
    private int GetRequiredProgressForType(BuildingType type)
    {
        return type switch
        {
            BuildingType.Shelter => 20,
            BuildingType.Farm => 30,
            BuildingType.WaterPurifier => 25,
            BuildingType.Infirmary => 40,
            _ => 100,
        };
    }

    public override string ToString()
    {
        return $"{Type} - Progress: {Progress}/{RequiredProgress} ({(IsComplete ? "Complete" : "In Progress")})";
    }
}