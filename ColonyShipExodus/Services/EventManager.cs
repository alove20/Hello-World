namespace ColonyShipExodus.Services;

/// <summary>
/// Handles the triggering of random events that affect the game state.
/// </summary>
public class EventManager
{
    private readonly Random _random = new();

    public void TriggerRandomEvent(Game game)
    {
        int chance = _random.Next(100);

        if (chance < 10) // 10% chance
        {
            GoodWeather(game);
        }
        else if (chance < 20) // 10% chance
        {
            ResourceCache(game);
        }
        else if (chance < 30) // 10% chance
        {
            MinorAccident(game);
        }
        else
        {
            // 70% chance of no event
            Console.WriteLine("The day passes uneventfully.");
        }
    }

    private void GoodWeather(Game game)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Event: Perfect weather! Food production is boosted for the day.");
        game.ResourceManager.AddResource(Models.ResourceType.Food, 10);
        Console.ResetColor();
    }

    private void ResourceCache(Game game)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Event: A scavenging party found a hidden cache of materials!");
        game.ResourceManager.AddResource(Models.ResourceType.BuildingMaterials, 15);
        Console.ResetColor();
    }
    
    private void MinorAccident(Game game)
    {
        if (game.Survivors.Any())
        {
            var survivor = game.Survivors[_random.Next(game.Survivors.Count)];
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Event: A minor accident occurred!");
            survivor.TakeDamage(15, "an accident");
            Console.ResetColor();
        }
    }
}