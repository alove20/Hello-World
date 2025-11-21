using ColonyShipExodus.Models;

namespace ColonyShipExodus;

public record GameEvent(string Description, Action<Colony> ApplyEffect);

/// <summary>
/// Manages random events that can occur each turn.
/// </summary>
public class EventManager
{
    private readonly List<GameEvent> _events = new();
    private readonly Random _random = new();

    public EventManager()
    {
        PopulateEvents();
    }

    private void PopulateEvents()
    {
        // Positive Events
        _events.Add(new GameEvent("You discovered a hidden supply cache from the ship!",
            colony =>
            {
                colony.AddResource(ResourceType.Food, 20);
                colony.AddResource(ResourceType.Medicine, 5);
                Console.WriteLine("Gained 20 Food and 5 Medicine.");
            }));
        
        _events.Add(new GameEvent("Clear skies and favorable weather boost morale.",
            colony =>
            {
                colony.Survivors.ForEach(s => s.Morale = Math.Min(100, s.Morale + 10));
                Console.WriteLine("All survivors gained 10 morale.");
            }));

        // Negative Events
        _events.Add(new GameEvent("A sudden alien sandstorm damages your stockpiles.",
            colony =>
            {
                int foodLost = Math.Min(15, colony.Resources[ResourceType.Food]);
                int waterLost = Math.Min(15, colony.Resources[ResourceType.Water]);
                colony.UseResource(ResourceType.Food, foodLost);
                colony.UseResource(ResourceType.Water, waterLost);
                Console.WriteLine($"Lost {foodLost} Food and {waterLost} Water.");
            }));
        
        _events.Add(new GameEvent("A strange local illness is spreading.",
            colony =>
            {
                var sickSurvivor = colony.Survivors[_random.Next(colony.Survivors.Count)];
                sickSurvivor.Health = Math.Max(0, sickSurvivor.Health - 20);
                Console.WriteLine($"{sickSurvivor.Name} has fallen ill and lost 20 health.");
            }));
        
        _events.Add(new GameEvent("Tools have gone missing. It will be harder to build.",
            colony =>
            {
                int matsLost = Math.Min(10, colony.Resources[ResourceType.BuildingMaterials]);
                colony.UseResource(ResourceType.BuildingMaterials, matsLost);
                Console.WriteLine($"Lost {matsLost} Building Materials.");
            }));
    }

    public void TriggerRandomEvent(Colony colony)
    {
        // 50% chance of an event happening each day
        if (_random.NextDouble() < 0.5)
        {
            var gameEvent = _events[_random.Next(_events.Count)];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n--- Daily Event: {gameEvent.Description} ---");
            gameEvent.ApplyEffect(colony);
            Console.ResetColor();
        }
    }
}