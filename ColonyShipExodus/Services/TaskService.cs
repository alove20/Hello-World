using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Services;

/// <summary>
/// Processes survivor tasks and their outcomes.
/// </summary>
public class TaskService
{
    private readonly Random _random = new();

    public void ProcessTasks(Colony colony, World world)
    {
        foreach (var survivor in colony.Survivors)
        {
            switch (survivor.CurrentTask)
            {
                case TaskType.Scavenging:
                    ProcessScavenging(colony, world);
                    break;
                case TaskType.Building:
                    ProcessBuilding(colony);
                    break;
                case TaskType.Farming:
                    // This could be tied to a Farm building's output
                    break;
                case TaskType.Resting:
                    survivor.Health += 5;
                    if (survivor.Health > 100) survivor.Health = 100;
                    break;
            }
        }
    }

    private void ProcessScavenging(Colony colony, World world)
    {
        // For simplicity, we assume scavenging happens in a random adjacent explored tile.
        // A more complex system would allow players to specify where to scavenge.
        
        var biome = world.Map[world.Width / 2, world.Height / 2].Biome; // Default to crash site for now
        
        switch (biome)
        {
            // Simplified scavenging results
            case BiomeType.Forest:
                colony.AddResource(ResourceType.Food, _random.Next(1, 4));
                colony.AddResource(ResourceType.BuildingMaterials, _random.Next(0, 3));
                break;
            case BiomeType.Mountains:
                colony.AddResource(ResourceType.BuildingMaterials, _random.Next(1, 4));
                break;
            case BiomeType.Ruins:
                colony.AddResource(ResourceType.Energy, _random.Next(0, 2));
                colony.AddResource(ResourceType.Medicine, _random.Next(0, 2));
                break;
            default: // Plains, CrashSite
                colony.AddResource(ResourceType.Food, _random.Next(0, 3));
                colony.AddResource(ResourceType.Water, _random.Next(0, 3));
                break;
        }
    }

    private void ProcessBuilding(Colony colony)
    {
        if (colony.InProgressBuilding != null && !colony.InProgressBuilding.IsComplete)
        {
            colony.InProgressBuilding.WorkApplied++;
            if (colony.InProgressBuilding.WorkApplied >= colony.InProgressBuilding.WorkRequired)
            {
                colony.InProgressBuilding.IsComplete = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nConstruction of {colony.InProgressBuilding.Type} is complete!");
                Console.ResetColor();
                colony.Buildings.Add(colony.InProgressBuilding);
                colony.InProgressBuilding = null;
            }
        }
    }
}