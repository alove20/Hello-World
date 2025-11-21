using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;
using ColonyShipExodus.Services;

namespace ColonyShipExodus;

/// <summary>
/// The main game engine class. Manages the game state, loop, and player interaction.
/// </summary>
public class Game
{
    private readonly World _world;
    private readonly Colony _colony;
    private readonly EventManager _eventManager;
    private readonly TaskService _taskService;
    private int _currentDay = 1;
    private bool _isGameOver = false;

    public Game()
    {
        _world = new World(10, 10);
        _colony = new Colony();
        _eventManager = new EventManager();
        _taskService = new TaskService();
    }

    public void Run()
    {
        Console.WriteLine("--- Colony Ship Exodus ---");
        Console.WriteLine("Your colony ship has crash-landed. You must guide the survivors to build a new home.");

        while (!_isGameOver)
        {
            DisplayStatus();
            GetPlayerInput();
            if (!_isGameOver)
            {
                AdvanceDay();
            }
        }

        Console.WriteLine("\n--- GAME OVER ---");
    }

    private void DisplayStatus()
    {
        Console.WriteLine("\n----------------------------------------");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Day: {_currentDay}");
        Console.ResetColor();

        Console.WriteLine("\n--- Resources ---");
        foreach (var resource in _colony.Resources)
        {
            Console.WriteLine($"{resource.Key}: {resource.Value}");
        }

        Console.WriteLine("\n--- Survivors ---");
        for (int i = 0; i < _colony.Survivors.Count; i++)
        {
            var s = _colony.Survivors[i];
            Console.WriteLine($"{i + 1}. {s.Name} | Health: {s.Health}/100 | Morale: {s.Morale}/100 | Task: {s.CurrentTask}");
        }
        
        Console.WriteLine("\n--- Buildings ---");
        if (_colony.Buildings.Any())
        {
            foreach (var b in _colony.Buildings.Where(b => b.IsComplete))
            {
                Console.WriteLine($"- {b.Type} (Complete)");
            }
        }
        if (_colony.InProgressBuilding != null)
        {
            var b = _colony.InProgressBuilding;
            Console.WriteLine($"- {b.Type} (In Progress: {b.WorkApplied}/{b.WorkRequired} work units)");
        }
        if (!_colony.Buildings.Any() && _colony.InProgressBuilding == null)
        {
             Console.WriteLine("No buildings yet.");
        }
        
        Console.WriteLine("----------------------------------------");
    }

    private void GetPlayerInput()
    {
        Console.WriteLine("\nChoose an action:");
        Console.WriteLine("1. Assign Survivor Tasks");
        Console.WriteLine("2. Build New Structure");
        Console.WriteLine("3. End Day");
        Console.WriteLine("4. Quit Game");

        bool validInput = false;
        while (!validInput)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AssignTasks();
                    validInput = true;
                    break;
                case "2":
                    BuildStructure();
                    validInput = true;
                    break;
                case "3":
                    Console.WriteLine("\nEnding the day...");
                    validInput = true;
                    break;
                case "4":
                    _isGameOver = true;
                    validInput = true;
                    break;
                default:
                    Console.WriteLine("Invalid input. Please choose a valid option.");
                    break;
            }
        }
    }
    
    private void AssignTasks()
    {
        Console.WriteLine("\n--- Assign Tasks ---");
        for(int i = 0; i < _colony.Survivors.Count; i++)
        {
            var s = _colony.Survivors[i];
            Console.WriteLine($"{i + 1}. {s.Name} (Current: {s.CurrentTask})");
        }
        Console.WriteLine("Enter survivor number to assign a task (or 'q' to go back):");
        string? input = Console.ReadLine();
        if(input?.ToLower() == "q") return;
        
        if (int.TryParse(input, out int survivorIndex) && survivorIndex > 0 && survivorIndex <= _colony.Survivors.Count)
        {
            var survivor = _colony.Survivors[survivorIndex - 1];
            Console.WriteLine($"\nChoose a task for {survivor.Name}:");
            var taskTypes = Enum.GetValues(typeof(TaskType)).Cast<TaskType>().ToArray();
            for(int i = 0; i < taskTypes.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {taskTypes[i]}");
            }
            
            Console.Write("> ");
            string? taskInput = Console.ReadLine();
            if (int.TryParse(taskInput, out int taskIndex) && taskIndex > 0 && taskIndex <= taskTypes.Length)
            {
                var selectedTask = taskTypes[taskIndex - 1];
                survivor.AssignTask(selectedTask);
                Console.WriteLine($"{survivor.Name} assigned to {selectedTask}.");
            }
            else
            {
                Console.WriteLine("Invalid task selection.");
            }
        }
        else
        {
            Console.WriteLine("Invalid survivor selection.");
        }
    }

    private void BuildStructure()
    {
        if (_colony.InProgressBuilding != null)
        {
            Console.WriteLine("You are already constructing a building. You must complete it first.");
            return;
        }

        Console.WriteLine("\n--- Build New Structure ---");
        var buildingTypes = Enum.GetValues(typeof(BuildingType)).Cast<BuildingType>().ToArray();
        for (int i = 0; i < buildingTypes.Length; i++)
        {
            var type = buildingTypes[i];
            var cost = Building.GetResourceCost(type);
            var costString = string.Join(", ", cost.Select(kvp => $"{kvp.Value} {kvp.Key}"));
            Console.WriteLine($"{i + 1}. {type} (Cost: {costString})");
        }
        
        Console.Write("Choose a structure to build (or 'q' to go back): ");
        string? input = Console.ReadLine();
        if(input?.ToLower() == "q") return;

        if (int.TryParse(input, out int buildingIndex) && buildingIndex > 0 && buildingIndex <= buildingTypes.Length)
        {
            var selectedType = buildingTypes[buildingIndex - 1];
            var resourceCost = Building.GetResourceCost(selectedType);

            bool canAfford = true;
            foreach (var cost in resourceCost)
            {
                if (_colony.Resources[cost.Key] < cost.Value)
                {
                    canAfford = false;
                    break;
                }
            }

            if (canAfford)
            {
                foreach (var cost in resourceCost)
                {
                    _colony.AddResource(cost.Key, -cost.Value);
                }
                _colony.InProgressBuilding = new Building(selectedType);
                Console.WriteLine($"Started construction of {selectedType}. Assign survivors to the 'Building' task to make progress.");
            }
            else
            {
                Console.WriteLine("Not enough resources to build this structure.");
            }
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    private void AdvanceDay()
    {
        // 1. Process survivor tasks (gathering, building, etc.)
        _taskService.ProcessTasks(_colony, _world);

        // 2. Process passive building effects
        ProcessBuildingEffects();
        
        // 3. Consume resources
        int foodConsumed = _colony.Survivors.Count;
        int waterConsumed = _colony.Survivors.Count;
        _colony.AddResource(ResourceType.Food, -foodConsumed);
        _colony.AddResource(ResourceType.Water, -waterConsumed);
        Console.WriteLine($"\nYour colony consumed {foodConsumed} food and {waterConsumed} water.");

        // 4. Update survivor status
        UpdateSurvivors();
        
        // 5. Trigger a random event
        _eventManager.TriggerRandomEvent(_colony);
        
        // 6. Check for win/loss conditions
        CheckGameOver();

        _currentDay++;
    }

    private void ProcessBuildingEffects()
    {
        foreach (var building in _colony.Buildings.Where(b => b.IsComplete))
        {
            switch (building.Type)
            {
                case BuildingType.Farm:
                    int foodProduced = 5;
                    _colony.AddResource(ResourceType.Food, foodProduced);
                    Console.WriteLine($"Your farm produced {foodProduced} food.");
                    break;
                case BuildingType.WaterPurifier:
                    int waterProduced = 10;
                    _colony.AddResource(ResourceType.Water, waterProduced);
                     Console.WriteLine($"Your water purifier produced {waterProduced} water.");
                    break;
            }
        }
    }
    
    private void UpdateSurvivors()
    {
        bool hasShelter = _colony.Buildings.Any(b => b.Type == BuildingType.Shelter && b.IsComplete);
        
        foreach (var s in _colony.Survivors)
        {
            // Morale effects
            if (!hasShelter) s.Morale -= 5; else s.Morale += 2;
            if (_colony.Resources[ResourceType.Food] <= 0) s.Morale -= 10;
            if (_colony.Resources[ResourceType.Water] <= 0) s.Morale -= 10;
            
            // Health effects
            if (_colony.Resources[ResourceType.Food] <= 0) s.Health -= 5;
            if (_colony.Resources[ResourceType.Water] <= 0) s.Health -= 5;
            
            if (s.Morale < 0) s.Morale = 0;
            if (s.Morale > 100) s.Morale = 100;
            if (s.Health < 0) s.Health = 0;
        }
    }

    private void CheckGameOver()
    {
        _colony.Survivors.RemoveAll(s => s.Health <= 0);

        if (!_colony.Survivors.Any())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAll of your survivors have perished. The colony is lost.");
            Console.ResetColor();
            _isGameOver = true;
        }

        // Win condition: Survive 30 days and build all structures
        if (_currentDay > 30 && _colony.Buildings.Count(b => b.IsComplete) >= Enum.GetValues(typeof(BuildingType)).Length)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nCongratulations! You have established a self-sufficient colony and secured a future for your people!");
            Console.ResetColor();
            _isGameOver = true;
        }
    }
}