using ColonyShipExodus.Models;
using System.Text;

namespace ColonyShipExodus;

/// <summary>
/// The main game engine, managing the game loop, state, and player interaction.
/// </summary>
public class Game
{
    private int _day = 1;
    private readonly World _world;
    private readonly Colony _colony;
    private readonly EventManager _eventManager;
    private bool _isGameOver;
    private string _turnSummary = "A new day dawns.";

    private static readonly Dictionary<BuildingType, Dictionary<ResourceType, int>> BuildingCosts = new()
    {
        [BuildingType.Shelter] = new() { { ResourceType.BuildingMaterials, 20 } },
        [BuildingType.Farm] = new() { { ResourceType.BuildingMaterials, 30 }, { ResourceType.Water, 10 } },
        [BuildingType.WaterPurifier] = new() { { ResourceType.BuildingMaterials, 25 }, { ResourceType.Energy, 5 } }
    };

    public Game()
    {
        _world = new World(15, 8);
        _colony = new Colony();
        _eventManager = new EventManager();
    }

    public void Run()
    {
        while (!_isGameOver)
        {
            Console.Clear();
            DisplayStatus();
            GetPlayerInput();
            if (_isGameOver) break;
            ProcessTurn();
            CheckEndConditions();
            _day++;
        }
        DisplayGameOver();
    }

    private void DisplayStatus()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n--- Day {_day} ---");
        Console.ResetColor();
        Console.WriteLine(_turnSummary);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n--- Colony Resources ---");
        foreach (var resource in _colony.Resources)
        {
            Console.WriteLine($"{resource.Key}: {resource.Value}");
        }

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n--- Constructed Buildings ---");
        if (_colony.Buildings.Any())
        {
            _colony.Buildings.ForEach(b => Console.WriteLine($"- {b}"));
        }
        else
        {
            Console.WriteLine("None");
        }
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n--- Survivors ---");
        for (int i = 0; i < _colony.Survivors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_colony.Survivors[i]}");
        }
        Console.ResetColor();
    }

    private void GetPlayerInput()
    {
        Console.WriteLine("\n--- Choose an action ---");
        Console.WriteLine("1. Assign tasks to survivors");
        Console.WriteLine("2. Construct a new building");
        Console.WriteLine("3. View world map");
        Console.WriteLine("4. End day and process turn");
        Console.WriteLine("5. Quit game");

        bool validInput = false;
        while (!validInput)
        {
            Console.Write("> ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AssignTasks();
                    validInput = true;
                    GetPlayerInput(); // Return to menu
                    break;
                case "2":
                    BuildStructure();
                    validInput = true;
                    GetPlayerInput(); // Return to menu
                    break;
                case "3":
                    _world.DisplayMap();
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ReadKey();
                    validInput = true;
                    GetPlayerInput(); // Return to menu
                    break;
                case "4":
                    validInput = true;
                    break;
                case "5":
                    _isGameOver = true;
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void AssignTasks()
    {
        Console.WriteLine("\n--- Assign Tasks ---");
        for (var i = 0; i < _colony.Survivors.Count; i++)
        {
            var survivor = _colony.Survivors[i];
            Console.WriteLine($"\nAssign task for {survivor.Name} ({survivor.Skill}):");
            Console.WriteLine($"Current Task: {survivor.AssignedTask}");
            Console.WriteLine("1. Idle | 2. Scavenging | 3. Building | 4. Farming | 5. Resting");
            
            bool validTask = false;
            while (!validTask)
            {
                Console.Write("> ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 5)
                {
                    survivor.AssignedTask = (TaskType)(choice-1); // Enum is 0-indexed
                    validTask = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
                }
            }
        }
        Console.WriteLine("\nAll tasks assigned. Returning to main menu.");
    }
    
    private void BuildStructure()
    {
        Console.WriteLine("\n--- Construct Building ---");
        var buildable = BuildingCosts.Keys.Where(b => !_colony.Buildings.Contains(b)).ToList();
        if (!buildable.Any())
        {
            Console.WriteLine("All available buildings have been constructed.");
            return;
        }

        for (int i = 0; i < buildable.Count; i++)
        {
            var type = buildable[i];
            string cost = string.Join(", ", BuildingCosts[type].Select(c => $"{c.Value} {c.Key}"));
            Console.WriteLine($"{i + 1}. {type} (Cost: {cost})");
        }
        Console.WriteLine($"{buildable.Count + 1}. Cancel");

        Console.Write("> ");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= buildable.Count)
        {
            var buildingToBuild = buildable[choice - 1];
            var cost = BuildingCosts[buildingToBuild];

            bool canAfford = cost.All(c => _colony.Resources[c.Key] >= c.Value);
            
            if (canAfford)
            {
                foreach (var resourceCost in cost)
                {
                    _colony.UseResource(resourceCost.Key, resourceCost.Value);
                }
                _colony.Buildings.Add(buildingToBuild);
                Console.WriteLine($"Successfully constructed {buildingToBuild}!");
            }
            else
            {
                Console.WriteLine("Not enough resources to construct this building.");
            }
        }
    }

    private void ProcessTurn()
    {
        var summary = new StringBuilder();

        // 1. Building Effects
        if (_colony.Buildings.Contains(BuildingType.Farm))
        {
            _colony.AddResource(ResourceType.Food, 10);
            summary.AppendLine("The farm produced 10 Food.");
        }
        if (_colony.Buildings.Contains(BuildingType.WaterPurifier))
        {
            _colony.AddResource(ResourceType.Water, 10);
            summary.AppendLine("The water purifier produced 10 Water.");
        }

        // 2. Process Survivor Tasks
        foreach (var survivor in _colony.Survivors)
        {
            int baseYield = 2;
            switch (survivor.AssignedTask)
            {
                case TaskType.Scavenging:
                    int scavengeYield = baseYield + (survivor.Skill == SurvivorSkill.Scavenger ? 2 : 0);
                    _colony.AddResource(ResourceType.BuildingMaterials, scavengeYield);
                    summary.AppendLine($"{survivor.Name} scavenged {scavengeYield} building materials.");
                    break;
                case TaskType.Farming:
                     int farmYield = baseYield + (survivor.Skill == SurvivorSkill.Farmer ? 3 : 0);
                    _colony.AddResource(ResourceType.Food, farmYield);
                    summary.AppendLine($"{survivor.Name} farmed {farmYield} food.");
                    break;
                case TaskType.Resting:
                    survivor.Health = Math.Min(100, survivor.Health + 10);
                    summary.AppendLine($"{survivor.Name} is resting and recovered some health.");
                    break;
            }
        }

        // 3. Resource Consumption
        int foodConsumed = _colony.Survivors.Count;
        int waterConsumed = _colony.Survivors.Count;

        if (!_colony.UseResource(ResourceType.Food, foodConsumed))
        {
            summary.AppendLine("Not enough food! Survivors are starving.");
            _colony.Survivors.ForEach(s => s.Health -= 10);
        }
        else
        {
            summary.AppendLine($"Colony consumed {foodConsumed} food.");
        }

        if (!_colony.UseResource(ResourceType.Water, waterConsumed))
        {
            summary.AppendLine("Not enough water! Survivors are dehydrated.");
            _colony.Survivors.ForEach(s => s.Health -= 10);
        }
        else
        {
            summary.AppendLine($"Colony consumed {waterConsumed} water.");
        }

        // 4. Random Event
        _eventManager.TriggerRandomEvent(_colony);
        
        // Remove dead survivors
        _colony.Survivors.RemoveAll(s => s.Health <= 0);
        
        _turnSummary = summary.ToString();
    }

    private void CheckEndConditions()
    {
        if (!_colony.Survivors.Any())
        {
            _isGameOver = true;
            _turnSummary += "\nAll survivors have perished. The colony is lost.";
        }
        // Example win condition: build all structures
        if (BuildingCosts.Keys.All(b => _colony.Buildings.Contains(b)))
        {
             _isGameOver = true;
            _turnSummary += "\nYou have built a self-sustaining colony! The survivors are safe. You win!";
        }
    }

    private void DisplayGameOver()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("--- GAME OVER ---");
        Console.ResetColor();
        Console.WriteLine($"You survived for {_day - 1} days.");
        Console.WriteLine(_turnSummary.Replace("You win!", "")); // Avoid double printing win message
        if (_turnSummary.Contains("You win!"))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You have built a self-sustaining colony! The survivors are safe. YOU WIN!");
            Console.ResetColor();
        }
    }
}