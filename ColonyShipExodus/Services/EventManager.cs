namespace ColonyShipExodus.Services;

public class EventManager
{
    private readonly Random _random = new();
    private readonly List<Action<Game>> _events;

    public EventManager()
    {
        _events = new List<Action<Game>>
        {
            GoodWeather,
            DiseaseOutbreak,
            ResourceDiscovery,
            ToolBreakage,
            AlienEncounter,
            NothingHappens
        };
    }

    public void TriggerRandomEvent(Game game)
    {
        if (_random.Next(0, 3) == 0) // 33% chance of an event each day
        {
            _events[_random.Next(_events.Count)](game);
        }
        else
        {
            NothingHappens(game);
        }
    }

    private void GoodWeather(Game game)
    {
        Console.WriteLine("\nEVENT: The weather is exceptionally good! Farming and scavenging yields are increased today.");
        // This effect will be applied in the main game loop's task processing
        game.EventBonuses["Farming"] = 2;
        game.EventBonuses["Scavenging"] = 2;
    }

    private void DiseaseOutbreak(Game game)
    {
        Console.WriteLine("\nEVENT: A strange illness is spreading through the camp.");
        var affectedSurvivor = game.Survivors[_random.Next(game.Survivors.Count)];
        int healthLoss = 20;
        affectedSurvivor.Health -= healthLoss;
        Console.WriteLine($"{affectedSurvivor.Name} has fallen ill and lost {healthLoss} health.");
    }

    private void ResourceDiscovery(Game game)
    {
        Console.WriteLine("\nEVENT: A scouting party found a hidden cache of supplies!");
        int materialsFound = _random.Next(10, 31);
        game.Resources[Enums.ResourceType.BuildingMaterials] += materialsFound;
        Console.WriteLine($"You gained {materialsFound} Building Materials.");
    }

    private void ToolBreakage(Game game)
    {
        Console.WriteLine("\nEVENT: A critical piece of equipment broke down!");
        int materialsLost = _random.Next(5, 16);
        if (game.Resources[Enums.ResourceType.BuildingMaterials] >= materialsLost)
        {
            game.Resources[Enums.ResourceType.BuildingMaterials] -= materialsLost;
            Console.WriteLine($"It will cost {materialsLost} Building Materials to repair.");
        }
        else
        {
            Console.WriteLine("You don't have enough materials to repair it, slowing down future construction.");
            game.EventPenalties["Building"] = 2;
        }
    }

    private void AlienEncounter(Game game)
    {
        Console.WriteLine("\nEVENT: Survivors encountered a territorial alien creature. They managed to escape, but are shaken.");
        foreach (var survivor in game.Survivors)
        {
            survivor.Morale -= 15;
        }
        Console.WriteLine("All survivors lost 15 morale.");
    }
    
    private void NothingHappens(Game game)
    {
        Console.WriteLine("\nThe day passes uneventfully.");
    }
}