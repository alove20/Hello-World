using ColonyShipExodus.Models;
using ColonyShipExodus.Services;

namespace ColonyShipExodus;

/// <summary>
/// The main game engine, orchestrating the game loop and state management.
/// </summary>
public class Game
{
    public int Day { get; private set; }
    public World World { get; }
    public ResourceManager ResourceManager { get; }
    public List<Survivor> Survivors { get; }
    public List<Building> Buildings { get; }
    private readonly EventManager _eventManager;
    private bool _isGameOver;

    public Game()
    {
        Day = 1;
        World = new World(10, 10);
        ResourceManager = new ResourceManager();
        Survivors = new List<Survivor>();
        Buildings = new List<Building>();
        _eventManager = new EventManager();
        _isGameOver = false;

        InitializeGame();
    }

    private void InitializeGame()
    {
        World.GenerateMap();
        // Add starting resources
        ResourceManager.AddResource(ResourceType.Food, 50);
        ResourceManager.AddResource(ResourceType.Water, 50);
        ResourceManager.AddResource(ResourceType.BuildingMaterials, 20);

        // Create initial survivors
        Survivors.Add(new Survivor("Dr. Aris", SurvivorSkill.Medicine));
        Survivors.Add(new Survivor("Jax", SurvivorSkill.Engineering));
        Survivors.Add(new Survivor("Kara", SurvivorSkill.Scavenging));
        Survivors.Add(new Survivor("Leo", SurvivorSkill.Farming));
        Survivors.Add(new Survivor("Zara", SurvivorSkill.Scavenging));
    }

    /// <summary>
    /// Runs the main game loop.
    /// </summary>
    public void Run()
    {
        Console.WriteLine("Welcome to Colony Ship Exodus!");
        Console.WriteLine("Your colony ship has crash-landed. You must guide the survivors to build a new home.");
        Console.WriteLine("Press Enter to begin...");
        Console.ReadLine();

        while (!_isGameOver)
        {
            DisplayStatus();
            DisplayMenu();
            HandlePlayerInput();
            CheckEndConditions();
        }

        Console.WriteLine("Game Over. Thanks for playing!");
    }

    private void DisplayStatus()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"--- Day {Day} ---");
        Console.ResetColor();

        Console.WriteLine("\n--- Resources ---");
        ResourceManager.DisplayResources();

        Console.WriteLine("\n--- Survivors ---");
        foreach (var survivor in Survivors)
        {
            Console.WriteLine(survivor);
        }

        Console.WriteLine("\n--- Buildings ---");
        if (Buildings.Any())
        {
            foreach (var building in Buildings)
            {
                Console.WriteLine(building);
            }
        }
        else
        {
            Console.WriteLine("No buildings constructed yet.");
        }
        Console.WriteLine();
    }

    private void DisplayMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("What would you like to do?");
        Console.WriteLine("1. Assign tasks to survivors");
        Console.WriteLine("2. Build new structures");
        Console.WriteLine("3. End day and process turn");
        Console.ResetColor();
    }

    private void HandlePlayerInput()
    {
        Console.Write("> ");
        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                AssignTasks();
                break;
            case "2":
                BuildStructure();
                break;
            case "3":
                ProcessDay();
                break;
            default:
                Console.WriteLine("Invalid input. Please try again.");
                Console.ReadLine();
                break;
        }
    }

    private void AssignTasks()
    {
        Console.WriteLine("\nAssign tasks:");
        foreach (var survivor in Survivors)
        {
            Console.WriteLine($"Assign task for {survivor.Name} (Skill: {survivor.Skill}):");
            Console.WriteLine("1. Scavenge  2. Build  3. Farm  4. Rest");
            bool valid = false;
            while (!valid)
            {
                Console.Write("> ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 4)
                {
                    survivor.CurrentTask = (SurvivorTask)(choice - 1);
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                }
            }
        }
        Console.WriteLine("Tasks assigned. Ready to end the day.");
        Console.ReadLine();
    }
    
    private void BuildStructure()
    {
        Console.WriteLine("\nWhat would you like to build?");
        Console.WriteLine($"1. Shelter (Cost: 20 Building Materials) - Improves morale");
        Console.WriteLine($"2. Farm (Cost: 15 Building Materials, 5 Food) - Unlocks farming task");
        Console.WriteLine($"3. Water Purifier (Cost: 15 Building Materials) - Produces water");
        Console.Write("> ");
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            BuildingType typeToBuild;
            Dictionary<ResourceType, int> costs;
            switch(choice)
            {
                case 1:
                    typeToBuild = BuildingType.Shelter;
                    costs = new() { { ResourceType.BuildingMaterials, 20 } };
                    break;
                case 2:
                    typeToBuild = BuildingType.Farm;
                    costs = new() { { ResourceType.BuildingMaterials, 15 }, { ResourceType.Food, 5 } };
                    break;
                case 3:
                    typeToBuild = BuildingType.WaterPurifier;
                    costs = new() { { ResourceType.BuildingMaterials, 15 } };
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
            
            if (ResourceManager.HasEnoughResources(costs))
            {
                ResourceManager.UseResources(costs);
                Buildings.Add(new Building(typeToBuild));
                Console.WriteLine($"{typeToBuild} construction started! Assign survivors to 'Build' to complete it.");
            }
            else
            {
                Console.WriteLine("Not enough resources to build.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }
        Console.ReadLine();
    }


    private void ProcessDay()
    {
        Console.WriteLine("\nEnding the day...");
        // Task processing
        ProcessTasks();
        
        // Resource Production from buildings
        ProcessBuildingEffects();
        
        // Resource Consumption
        int foodConsumed = Survivors.Count;
        int waterConsumed = Survivors.Count;
        ResourceManager.UseResource(ResourceType.Food, foodConsumed);
        ResourceManager.UseResource(ResourceType.Water, waterConsumed);
        Console.WriteLine($"Colony consumed {foodConsumed} food and {waterConsumed} water.");
        
        // Update Survivors
        UpdateSurvivors();
        
        // Random Event
        _eventManager.TriggerRandomEvent(this);
        
        Day++;
        Console.WriteLine("A new day dawns.");
        Console.ReadLine();
    }
    
    private void ProcessTasks()
    {
        var random = new Random();
        foreach (var survivor in Survivors)
        {
            switch (survivor.CurrentTask)
            {
                case SurvivorTask.Scavenging:
                    int materialsFound = random.Next(1, 4) + (survivor.Skill == SurvivorSkill.Scavenging ? 2 : 0);
                    ResourceManager.AddResource(ResourceType.BuildingMaterials, materialsFound);
                    Console.WriteLine($"{survivor.Name} scavenged {materialsFound} Building Materials.");
                    break;
                case SurvivorTask.Build:
                    var buildingInProgress = Buildings.FirstOrDefault(b => !b.IsComplete);
                    if (buildingInProgress != null)
                    {
                        int progress = 2 + (survivor.Skill == SurvivorSkill.Engineering ? 2 : 0);
                        buildingInProgress.AddProgress(progress);
                        Console.WriteLine($"{survivor.Name} worked on the {buildingInProgress.Type}, adding {progress} progress.");
                    }
                    break;
                case SurvivorTask.Farm:
                    if (Buildings.Any(b => b.Type == BuildingType.Farm && b.IsComplete))
                    {
                         int foodFarmed = random.Next(2, 5) + (survivor.Skill == SurvivorSkill.Farming ? 2 : 0);
                         ResourceManager.AddResource(ResourceType.Food, foodFarmed);
                         Console.WriteLine($"{survivor.Name} farmed {foodFarmed} food.");
                    }
                    else
                    {
                        Console.WriteLine($"{survivor.Name} tried to farm, but no completed Farm exists.");
                    }
                    break;
                case SurvivorTask.Rest:
                    survivor.Rest();
                    Console.WriteLine($"{survivor.Name} is resting, recovering health and morale.");
                    break;
            }
        }
    }
    
    private void ProcessBuildingEffects()
    {
        foreach(var building in Buildings.Where(b => b.IsComplete))
        {
            if (building.Type == BuildingType.WaterPurifier)
            {
                int waterProduced = 5;
                ResourceManager.AddResource(ResourceType.Water, waterProduced);
                Console.WriteLine($"Water Purifier produced {waterProduced} water.");
            }
        }
    }
    
    private void UpdateSurvivors()
    {
        bool hasShelter = Buildings.Any(b => b.Type == BuildingType.Shelter && b.IsComplete);
        
        foreach(var survivor in Survivors)
        {
            if (ResourceManager.GetResourceCount(ResourceType.Food) == 0)
            {
                survivor.TakeDamage(10, "Starvation");
            }
            if (ResourceManager.GetResourceCount(ResourceType.Water) == 0)
            {
                survivor.TakeDamage(10, "Dehydration");
            }

            if(hasShelter) survivor.ChangeMorale(5); else survivor.ChangeMorale(-5);
        }
        Survivors.RemoveAll(s => s.Health <= 0); // Remove dead survivors
    }


    private void CheckEndConditions()
    {
        if (Survivors.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAll your survivors have perished. The colony is lost.");
            _isGameOver = true;
        }

        if (Day > 50) // Example win condition
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nYou have survived for 50 days and established a stable colony. You win!");
            _isGameOver = true;
        }
    }
}