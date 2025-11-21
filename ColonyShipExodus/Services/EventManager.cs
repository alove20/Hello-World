using ColonyShipExodus.Models;
using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Services;

/// <summary>
/// Handles random events that occur in the game.
/// </summary>
public class EventManager
{
    private readonly Random _random = new();

    public void TriggerRandomEvent(Colony colony)
    {
        // 25% chance of an event happening each day
        if (_random.Next(0, 4) != 0) return;

        int eventType = _random.Next(0, 4);
        Console.ForegroundColor = ConsoleColor.Yellow;
        switch (eventType)
        {
            case 0:
                Console.WriteLine("\nEVENT: A sudden storm swept through the area, damaging some supplies.");
                int foodLost = _random.Next(5, 16);
                colony.AddResource(ResourceType.Food, -foodLost);
                Console.WriteLine($"You lost {foodLost} Food.");
                break;
            case 1:
                Console.WriteLine("\nEVENT: A scavenger party found a hidden cache of old-world supplies!");
                int materialsGained = _random.Next(10, 26);
                colony.AddResource(ResourceType.BuildingMaterials, materialsGained);
                Console.WriteLine($"You gained {materialsGained} Building Materials.");
                break;
            case 2:
                if (colony.Survivors.Any())
                {
                    var survivor = colony.Survivors[_random.Next(colony.Survivors.Count)];
                    Console.WriteLine($"\nEVENT: {survivor.Name} has contracted a strange alien illness.");
                    survivor.Health -= 20;
                    if (survivor.Health < 0) survivor.Health = 0;
                    Console.WriteLine($"{survivor.Name}'s health has been reduced.");
                }
                break;
            case 3:
                 Console.WriteLine("\nEVENT: A period of unusually good weather has lifted everyone's spirits.");
                 foreach(var s in colony.Survivors)
                 {
                     s.Morale += 10;
                     if(s.Morale > 100) s.Morale = 100;
                 }
                 Console.WriteLine("All survivors gain 10 morale.");
                break;
        }
        Console.ResetColor();
    }
}