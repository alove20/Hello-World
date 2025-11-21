using ColonyShipExodus.Models;
using ColonyShipExodus.World;

namespace ColonyShipExodus;

/// <summary>
/// The main engine for the game, managing the game state, loop, and player interactions.
/// </summary>
public class GameManager
{
    private int _day = 1;
    private readonly World.World _world;
    private readonly Colony _colony;
    private readonly Random _random = new();
    private bool _isGameOver = false;

    public GameManager()
    {
        _world = new World.World(10, 10);
        _colony = new Colony();
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Add initial survivors
        _colony.Survivors.Add(new Survivor("Dr. Aris Thorne", SurvivorSkill.Medicine));
        _colony.Survivors.Add(new Survivor("Jia 'Wrench' Li", SurvivorSkill.Engineering));
        _colony.Survivors.Add(new Survivor("Kenji Tanaka", SurvivorSkill.Farming));
        _colony.Survivors.Add(new Survivor("Maria Flores", SurvivorSkill.Scavenging));

        // Add starting resources
        _colony.Inventory.AddResource(ResourceType.Food, 20);
        _colony.Inventory.AddResource(ResourceType.Water, 20);
        _colony.Inventory.AddResource(ResourceType.BuildingMaterials, 10);
    }

    /// <summary>
    /// Runs the main game loop.
    /// </summary>
    public void RunGame()
    {
        ConsoleUI.DisplayIntro();

        while (!_isGameOver)
        {
            DisplayStatus();
            PlayerTurn();
            ProcessDayEnd();
            CheckEndConditions();
            _day++;
        }

        ConsoleUI.DisplayGameOver(_colony.Survivors.Count);
    }

    /// <summary>
    /// Displays the current status of the colony.
    /// </summary>
    private void DisplayStatus()
    {
        ConsoleUI.Clear();
        ConsoleUI.DisplayHeader($"Day {_day}");
        _colony.DisplayStatus();
        Console.WriteLine();
    }

    /// <summary>
    /// Manages the player's actions for the turn.
    /// </summary>
    private void PlayerTurn()
    {
        ConsoleUI.DisplayHeader("Commander's Orders");
        ConsoleUI.DisplayMessage("Assign tasks to survivors for the day:");

        foreach (var survivor in _colony.Survivors)
        {
            ConsoleUI.DisplaySurvivorTaskMenu(survivor);
            var choice = ConsoleUI.GetPlayerInput(4);

            survivor.CurrentTask = choice switch
            {
                1 => SurvivorTask.GatherFood,
                2 => SurvivorTask.GatherWater,
                3 => SurvivorTask.ScavengeMaterials,
                4 => SurvivorTask.Rest,
                _ => SurvivorTask.Rest
            };
            ConsoleUI.DisplayMessage($"{survivor.Name} is assigned to {survivor.CurrentTask}.");
        }
        ConsoleUI.Pause();
    }

    /// <summary>
    /// Processes the end-of-day events and updates.
    /// </summary>
    private void ProcessDayEnd()
    {
        ConsoleUI.Clear();
        ConsoleUI.DisplayHeader($"End of Day {_day}");

        // 1. Gather resources based on tasks
        ProcessGathering();
        
        // 2. Consume resources
        ProcessConsumption();

        // 3. Handle random events
        HandleRandomEvent();
        
        // 4. Update survivor status (resting, etc.)
        UpdateSurvivors();

        ConsoleUI.Pause();
    }

    private void ProcessGathering()
    {
        ConsoleUI.DisplaySubHeader("Resource Report");
        foreach (var survivor in _colony.Survivors)
        {
            int amount = _random.Next(2, 6); // Base amount
            switch (survivor.CurrentTask)
            {
                case SurvivorTask.GatherFood:
                    _colony.Inventory.AddResource(ResourceType.Food, amount);
                    ConsoleUI.DisplayMessage($"{survivor.Name} gathered {amount} Food.");
                    break;
                case SurvivorTask.GatherWater:
                    _colony.Inventory.AddResource(ResourceType.Water, amount);
                    ConsoleUI.DisplayMessage($"{survivor.Name} gathered {amount} Water.");
                    break;
                case SurvivorTask.ScavengeMaterials:
                    _colony.Inventory.AddResource(ResourceType.BuildingMaterials, amount / 2); // Materials are scarcer
                    ConsoleUI.DisplayMessage($"{survivor.Name} scavenged {amount / 2} Building Materials.");
                    break;
            }
        }
        Console.WriteLine();
    }

    private void ProcessConsumption()
    {
        ConsoleUI.DisplaySubHeader("Consumption Report");
        int foodNeeded = _colony.Survivors.Count;
        int waterNeeded = _colony.Survivors.Count;

        _colony.Inventory.UseResource(ResourceType.Food, foodNeeded);
        _colony.Inventory.UseResource(ResourceType.Water, waterNeeded);

        ConsoleUI.DisplayMessage($"Colony consumed {foodNeeded} Food and {waterNeeded} Water.");

        if (_colony.Inventory.GetResourceCount(ResourceType.Food) <= 0)
        {
            ConsoleUI.DisplayWarning("Warning: Food supplies have run out! Survivors are starving.");
            foreach (var s in _colony.Survivors) s.Health -= 10;
        }
        if (_colony.Inventory.GetResourceCount(ResourceType.Water) <= 0)
        {
            ConsoleUI.DisplayWarning("Warning: Water supplies have run out! Survivors are dehydrated.");
            foreach (var s in _colony.Survivors) s.Health -= 15;
        }
        Console.WriteLine();
    }
    
    private void HandleRandomEvent()
    {
        int chance = _random.Next(1, 101);
        if (chance <= 25) // 25% chance of an event
        {
            ConsoleUI.DisplaySubHeader("An Event Occurs!");
            int eventType = _random.Next(1, 4);
            switch (eventType)
            {
                case 1:
                    ConsoleUI.DisplayMessage("A sudden storm damages your supplies! You lose some materials.");
                    _colony.Inventory.UseResource(ResourceType.BuildingMaterials, 5);
                    break;
                case 2:
                    ConsoleUI.DisplayMessage("Good news! A scavenging party found a small, hidden cache of medicine.");
                    _colony.Inventory.AddResource(ResourceType.Medicine, 3);
                    break;
                case 3:
                    var sickSurvivor = _colony.Survivors[_random.Next(_colony.Survivors.Count)];
                    sickSurvivor.Health -= 20;
                    ConsoleUI.DisplayWarning($"{sickSurvivor.Name} has fallen ill with an alien fever!");
                    break;
            }
             Console.WriteLine();
        }
    }

    private void UpdateSurvivors()
    {
         foreach(var s in _colony.Survivors)
         {
             if (s.CurrentTask == SurvivorTask.Rest)
             {
                 s.Health += 10;
             }
             s.Health = Math.Clamp(s.Health, 0, 100);
         }
    }

    /// <summary>
    /// Checks for win/loss conditions.
    /// </summary>
    private void CheckEndConditions()
    {
        // Remove dead survivors
        _colony.Survivors.RemoveAll(s =>
        {
            if (s.Health <= 0)
            {
                ConsoleUI.DisplayWarning($"{s.Name} has perished.");
                return true;
            }
            return false;
        });

        if (_colony.Survivors.Count == 0)
        {
            _isGameOver = true;
        }
    }
}