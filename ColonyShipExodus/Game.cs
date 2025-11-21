using ColonyShipExodus.Data;
using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;
using ColonyShipExodus.Services;

namespace ColonyShipExodus;

public class Game
{
    public int Day { get; private set; } = 1;
    public Map WorldMap { get; }
    public List<Survivor> Survivors { get; } = new();
    public Dictionary<ResourceType, int> Resources { get; } = new();
    public List<Building> Buildings { get; } = new();
    public bool IsGameOver { get; private set; } = false;
    public string GameOverMessage { get; private set; } = "";
    
    // Event-related flags
    public bool IsBadWeather { get; set; } = false;

    private readonly EventManager _eventManager = new();
    private readonly List<string> _dailyLog = new();

    public Game()
    {
        // Initialize Game State
        var worldGenerator = new WorldGenerator();
        WorldMap = worldGenerator.GenerateMap(10, 10);

        // Add initial survivors
        Survivors.Add(new Survivor("Commander Eva"));
        Survivors.Add(new Survivor("Dr. Aris Thorne"));
        Survivors.Add(new Survivor("Engineer Jax"));
        Survivors.Add(new Survivor("Scout Lena"));

        // Add starting resources
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            Resources[type] = 0;
        }
        Resources[ResourceType.Food] = 20;
        Resources[ResourceType.Water] = 20;
        Resources[ResourceType.BuildingMaterials] = 10;
    }

    public void Run()
    {
        DisplayIntro();
        while (!IsGameOver)
        {
            DisplayStatus();
            ProcessPlayerInput();
            if (!IsGameOver) // Player might quit
            {
                UpdateDay();
                DisplayDailyLog();
                CheckEndConditions();
            }
        }
        Console.WriteLine("\n================ GAME OVER ================");
        Console.WriteLine(GameOverMessage);
    }

    private void DisplayIntro()
    {
        Console.Clear();
        Console.WriteLine("=== Colony Ship Exodus ===");
        Console.WriteLine("Your colony ship has crash-landed on an unknown alien world.");
        Console.WriteLine("As the commander, you must lead the few survivors to establish a new home.");
        Console.WriteLine("Manage your resources, explore the surroundings, and build a sustainable colony.");
        Console.WriteLine("Press any key to begin...");
        Console.ReadKey();
    }

    private void DisplayStatus()
    {
        Console.Clear();
        Console.WriteLine($"--- Day {Day} ---");
        
        Console.WriteLine("\n--- Resources ---");
        foreach (var resource in Resources)
        {
            Console.WriteLine($"{resource.Key}: {resource.Value}");
        }

        Console.WriteLine("\n--- Survivors ---");
        for(int i = 0; i < Survivors.Count; i++)
        {
            var s = Survivors[i];
            Console.WriteLine($"{i + 1}. {s.Name} | Health: {s.Health}/100 | Morale: {s.Morale}/100 | Task: {s.CurrentTask}");
        }

        Console.WriteLine("\n--- Buildings ---");
        if (Buildings.Count == 0)
        {
            Console.WriteLine("None");
        }
        else
        {
            foreach (var b in Buildings)
            {
                string status = b.IsCompleted ? "Completed" : $"In Progress ({b.Progress}/{b.RequiredProgress})";
                Console.WriteLine($"- {b.Type}: {status}");
            }
        }
        Console.WriteLine("\n-----------------");
    }

    private void DisplayDailyLog()
    {
        Console.WriteLine("\n--- Daily Log ---");
        if (_dailyLog.Count == 0)
        {
            Console.WriteLine("A quiet day.");
        }
        else
        {
            foreach (var entry in _dailyLog)
            {
                Console.WriteLine($"- {entry}");
            }
        }
        Console.WriteLine("-----------------\n");
        Console.WriteLine("Press any key to continue to the next day...");
        Console.ReadKey();
    }

    private void ProcessPlayerInput()
    {
        bool turnEnded = false;
        while (!turnEnded)
        {
            Console.WriteLine("\nWhat are your orders, Commander?");
            Console.WriteLine("1. Assign Tasks");
            Console.WriteLine("2. Build");
            Console.WriteLine("3. View Map");
            Console.WriteLine("4. End Day");
            Console.WriteLine("5. Quit Game");
            Console.Write("> ");
            
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AssignTasks();
                    break;
                case "2":
                    Build();
                    break;
                case "3":
                    DisplayMap();
                    break;
                case "4":
                    turnEnded = true;
                    break;
                case "5":
                    IsGameOver = true;
                    GameOverMessage = "You have resigned your command.";
                    turnEnded = true;
                    break;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }

    private void AssignTasks()
    {
        Console.WriteLine("\n--- Assign Tasks ---");
        Console.WriteLine("Select a survivor to assign a task (or 0 to go back):");
        for(int i = 0; i < Survivors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Survivors[i].Name} (Current: {Survivors[i].CurrentTask})");
        }
        Console.Write("> ");
        if (!int.TryParse(Console.ReadLine(), out int survivorIndex) || survivorIndex < 0 || survivorIndex > Survivors.Count)
        {
            Console.WriteLine("Invalid survivor selection.");
            return;
        }

        if (survivorIndex == 0) return;

        var survivor = Survivors[survivorIndex - 1];

        Console.WriteLine($"\nSelect a task for {survivor.Name}:");
        var tasks = Enum.GetValues<SurvivorTask>();
        for(int i = 0; i < tasks.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {tasks[i]}");
        }
        Console.Write("> ");
        if (!int.TryParse(Console.ReadLine(), out int taskIndex) || taskIndex < 1 || taskIndex > tasks.Length)
        {
            Console.WriteLine("Invalid task selection.");
            return;
        }

        survivor.CurrentTask = tasks[taskIndex - 1];
        Console.WriteLine($"{survivor.Name} assigned to {survivor.CurrentTask}.");
        DisplayStatus(); // Refresh status screen
    }

    private void Build()
    {
        Console.WriteLine("\n--- Build Menu ---");
        var availableBlueprints = BuildingBlueprints.Blueprints
            .Where(bp => !Buildings.Any(b => b.Type == bp.Key))
            .ToList();

        if (availableBlueprints.Count == 0)
        {
            Console.WriteLine("All available structures are already built or under construction.");
            return;
        }
        
        Console.WriteLine("Select a structure to build (or 0 to go back):");
        for(int i = 0; i < availableBlueprints.Count; i++)
        {
            var bp = availableBlueprints[i].Value;
            var costStr = string.Join(", ", bp.Cost.Select(c => $"{c.Value} {c.Key}"));
            Console.WriteLine($"{i + 1}. {bp.Type} (Cost: {costStr}, Time: {bp.RequiredProgress} man-days)");
        }
        Console.Write("> ");

        if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > availableBlueprints.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }
        if (choice == 0) return;

        var selectedBlueprint = availableBlueprints[choice - 1].Value;

        // Check resources
        bool canAfford = true;
        foreach (var cost in selectedBlueprint.Cost)
        {
            if (Resources[cost.Key] < cost.Value)
            {
                canAfford = false;
                break;
            }
        }

        if (canAfford)
        {
            // Deduct resources
            foreach (var cost in selectedBlueprint.Cost)
            {
                Resources[cost.Key] -= cost.Value;
            }
            // Add building project
            Buildings.Add(BuildingBlueprints.CreateBuilding(selectedBlueprint.Type));
            Console.WriteLine($"Construction started for {selectedBlueprint.Type}. Assign survivors to 'Building' task to make progress.");
        }
        else
        {
            Console.WriteLine("Not enough resources to build.");
        }
        DisplayStatus();
    }
    
    private void DisplayMap()
    {
        Console.WriteLine("\n--- Exploration Map ---");
        Console.WriteLine("Legend: @: Crash Site, [First Letter]: Explored Biome, ?: Unexplored");

        for (int y = 0; y < WorldMap.Height; y++)
        {
            for (int x = 0; x < WorldMap.Width; x++)
            {
                if (x == WorldMap.CrashSiteLocation.X && y == WorldMap.CrashSiteLocation.Y)
                {
                    Console.Write("[@]");
                }
                else if (WorldMap.Grid[x, y].IsExplored)
                {
                    Console.Write($"[{WorldMap.Grid[x,y].Biome.ToString()[0]}]");
                }
                else
                {
                    Console.Write("[?]");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nExplored Tiles:");
        for (int y = 0; y < WorldMap.Height; y++)
        {
            for (int x = 0; x < WorldMap.Width; x++)
            {
                if (WorldMap.Grid[x, y].IsExplored)
                {
                    var resources = WorldMap.Grid[x, y].Resources;
                    string resStr = resources.Any() ? string.Join(", ", resources.Select(r => $"{r.Value} {r.Key}")) : "None";
                    Console.WriteLine($"- ({x},{y}): {WorldMap.Grid[x, y].Biome}. Resources: {resStr}");
                }
            }
        }
    }

    private void UpdateDay()
    {
        Day++;
        _dailyLog.Clear();
        IsBadWeather = false; // Reset weather flag

        // 1. Trigger Events
        _eventManager.TriggerRandomEvent(this, _dailyLog);

        // 2. Process Survivor Tasks
        ProcessTasks();

        // 3. Building Production
        ProcessBuildingEffects();
        
        // 4. Resource Consumption
        int foodConsumed = Survivors.Count;
        int waterConsumed = Survivors.Count;
        Resources[ResourceType.Food] -= foodConsumed;
        Resources[ResourceType.Water] -= waterConsumed;
        _dailyLog.Add($"Colony consumed {foodConsumed} Food and {waterConsumed} Water.");

        if (Resources[ResourceType.Food] < 0)
        {
            _dailyLog.Add("WARNING: Food shortage! Survivors are starving.");
            foreach (var s in Survivors) s.Health -= 10;
            Resources[ResourceType.Food] = 0;
        }
        if (Resources[ResourceType.Water] < 0)
        {
            _dailyLog.Add("WARNING: Water shortage! Survivors are dehydrated.");
            foreach (var s in Survivors) s.Health -= 10;
            Resources[ResourceType.Water] = 0;
        }

        // 5. Update Survivor Status
        foreach (var survivor in Survivors)
        {
            survivor.UpdateStatus();
        }
        
        // Remove dead survivors
        int deaths = Survivors.RemoveAll(s => s.Health <= 0);
        if (deaths > 0)
        {
            _dailyLog.Add($"{deaths} survivor(s) perished today.");
            // Morale hit for everyone
            foreach(var s in Survivors) s.Morale = Math.Max(0, s.Morale - 20);
        }
    }

    private void ProcessTasks()
    {
        var scavengers = Survivors.Where(s => s.CurrentTask == SurvivorTask.Scavenging).ToList();
        if (scavengers.Any())
        {
            // Simple scavenging logic: find resources at crash site
            int foodFound = new Random().Next(0, 2 * scavengers.Count);
            int waterFound = new Random().Next(0, 3 * scavengers.Count);
            if (IsBadWeather)
            {
                foodFound /= 2;
                waterFound /= 2;
            }
            Resources[ResourceType.Food] += foodFound;
            Resources[ResourceType.Water] += waterFound;
            if (foodFound > 0 || waterFound > 0)
                _dailyLog.Add($"Scavengers found {foodFound} Food and {waterFound} Water near the crash site.");
        }

        var builders = Survivors.Where(s => s.CurrentTask == SurvivorTask.Building).ToList();
        if (builders.Any())
        {
            var project = Buildings.FirstOrDefault(b => !b.IsCompleted);
            if (project != null)
            {
                project.AddProgress(builders.Count);
                _dailyLog.Add($"{builders.Count} survivor(s) worked on the {project.Type}. Progress: {project.Progress}/{project.RequiredProgress}.");
                if (project.IsCompleted)
                {
                    _dailyLog.Add($"Construction of {project.Type} is complete!");
                }
            }
            else
            {
                _dailyLog.Add("Builders had no project to work on.");
            }
        }

        var farmers = Survivors.Where(s => s.CurrentTask == SurvivorTask.Farming).ToList();
        if (farmers.Any())
        {
            var farm = Buildings.FirstOrDefault(b => b.Type == BuildingType.Farm && b.IsCompleted);
            if (farm != null)
            {
                int extraFood = farmers.Count * 2; // Each farmer produces 2 extra food
                Resources[ResourceType.Food] += extraFood;
                _dailyLog.Add($"{farmers.Count} survivor(s) working the farm produced an extra {extraFood} Food.");
            }
            else
            {
                _dailyLog.Add("Farmers had no completed Farm to work on.");
            }
        }
    }

    private void ProcessBuildingEffects()
    {
        foreach (var building in Buildings.Where(b => b.IsCompleted))
        {
            switch(building.Type)
            {
                case BuildingType.Farm:
                    int foodProduced = 5;
                    Resources[ResourceType.Food] += foodProduced;
                    _dailyLog.Add($"The Farm produced {foodProduced} Food.");
                    break;
                case BuildingType.WaterPurifier:
                    int waterProduced = 5;
                    Resources[ResourceType.Water] += waterProduced;
                    _dailyLog.Add($"The Water Purifier produced {waterProduced} Water.");
                    break;
                case BuildingType.Infirmary:
                    // Passive health boost for resting survivors
                    foreach(var s in Survivors.Where(s => s.CurrentTask == SurvivorTask.Resting))
                    {
                        s.Health = Math.Min(100, s.Health + 5); // Extra 5 health
                    }
                    if (Survivors.Any(s => s.CurrentTask == SurvivorTask.Resting))
                        _dailyLog.Add("The Infirmary provided better care for resting survivors.");
                    break;
            }
        }
    }

    private void CheckEndConditions()
    {
        if (!Survivors.Any())
        {
            IsGameOver = true;
            GameOverMessage = "All your survivors have perished. The colony is lost.";
            return;
        }

        bool allBuildingsComplete = BuildingBlueprints.Blueprints.Keys
            .All(bt => Buildings.Any(b => b.Type == bt && b.IsCompleted));

        if (allBuildingsComplete && Resources[ResourceType.Food] > 100 && Resources[ResourceType.Water] > 100)
        {
            IsGameOver = true;
            GameOverMessage = "You have built a self-sufficient colony! The survivors are safe and have a future on this new world. You have won!";
            return;
        }
    }
}