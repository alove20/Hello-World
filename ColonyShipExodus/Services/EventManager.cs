using ColonyShipExodus.Models;

namespace ColonyShipExodus.Services;

public class EventManager
{
    private readonly Random _random = new();
    private readonly Action<Game, List<string>>[] _events;

    public EventManager()
    {
        _events = new Action<Game, List<string>>[]
        {
            GoodWeather,
            BadWeather,
            ResourceDiscovery,
            SurvivorSickness,
            GoodMoraleBoost,
        };
    }

    public void TriggerRandomEvent(Game game, List<string> eventLog)
    {
        if (_random.Next(0, 100) < 30) // 30% chance of an event each day
        {
            int eventIndex = _random.Next(0, _events.Length);
            _events[eventIndex](game, eventLog);
        }
    }

    private void GoodWeather(Game game, List<string> eventLog)
    {
        eventLog.Add("EVENT: The weather is unusually pleasant. Everyone's morale has improved slightly.");
        foreach (var survivor in game.Survivors)
        {
            survivor.Morale = Math.Min(100, survivor.Morale + 5);
        }
    }

    private void BadWeather(Game game, List<string> eventLog)
    {
        eventLog.Add("EVENT: A harsh storm rolls in. Scavenging is less effective today.");
        // Logic to reduce scavenging effectiveness is handled in the main game loop's resource gathering phase
        game.IsBadWeather = true;
    }

    private void ResourceDiscovery(Game game, List<string> eventLog)
    {
        var resourceType = (Enums.ResourceType)_random.Next(0, 4);
        int amount = _random.Next(5, 16);
        game.Resources[resourceType] += amount;
        eventLog.Add($"EVENT: A scavenger party found a hidden cache of {amount} {resourceType}!");
    }
    
    private void SurvivorSickness(Game game, List<string> eventLog)
    {
        if (game.Survivors.Count == 0) return;
        
        var sickSurvivor = game.Survivors[_random.Next(game.Survivors.Count)];
        sickSurvivor.Health -= 20;
        eventLog.Add($"EVENT: {sickSurvivor.Name} has fallen ill! Their health has decreased.");
    }

    private void GoodMoraleBoost(Game game, List<string> eventLog)
    {
        eventLog.Add("EVENT: A survivor tells a captivating story from Old Earth, boosting morale.");
        foreach (var survivor in game.Survivors)
        {
            survivor.Morale = Math.Min(100, survivor.Morale + 10);
        }
    }
}