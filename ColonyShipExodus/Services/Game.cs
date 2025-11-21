using ColonyShipExodus.Enums;
using ColonyShipExodus.Models;
using System.Text;

namespace ColonyShipExodus.Services;

public class Game
{
    public int Day { get; private set; }
    public Dictionary<ResourceType, int> Resources { get; }
    public List<Survivor> Survivors { get; }
    public List<Building> Buildings { get; }
    public MapTile[,] Map { get; }
    public Dictionary<string, int> EventBonuses { get; private set; }
    public Dictionary<string, int> EventPenalties { get; private set; }

    private readonly WorldGenerator _worldGenerator;
    private readonly EventManager _eventManager;
    private const int MapWidth = 10;
    private const int MapHeight = 10;
    private bool _gameOver;

    public Game()
    {
        Day = 1;
        _worldGenerator = new WorldGenerator();
        _eventManager = new EventManager();
        Map = _worldGenerator.GenerateMap(MapWidth, MapHeight);
        _gameOver = false;
        
        Resources = new Dictionary<ResourceType, int>
        {
            { ResourceType.Food, 50 },
            { ResourceType.Water, 50 },
            { ResourceType.Medicine, 10 },
            { ResourceType.BuildingMaterials, 20 },
            { ResourceType.Energy, 0 }
        };

        Survivors = new List<Survivor>
        {
            new("Jian Li", SurvivorSkill.Builder),
            new("Ravi Singh", SurvivorSkill.Farmer),
            new("Elena Petrova", SurvivorSkill.Medic),
            new("Marcus Cole", SurvivorSkill.Scavenger)
        };
        
        Buildings = new List<Building>();
        EventBonuses = new Dictionary<string, int>();
        EventPenalties = new Dictionary<string, int>();
    }

    public void Run()
    {
        Console.WriteLine("--- Colony Ship Exodus ---");
        Console.WriteLine("Your colony ship has crash-landed. You must guide the survivors to build a new home.");
        Console.WriteLine("Type 'help' for a list of commands.");

        while (!_gameOver)
        {
            DisplayStatus();
            HandlePlayerInput();
            if (!_gameOver)
            {
                AdvanceDay();
                CheckEndConditions();
            }
        }

        Console.WriteLine("\n--- GAME OVER ---");
    }

    private void DisplayStatus()
    {
        Console.WriteLine("\n----------------------------------------");
        Console.WriteLine($"--- Day: {Day} ---");
        Console.WriteLine("\n--- Resources ---");
        foreach (var resource in Resources)
        {
            Console.WriteLine($"{resource.Key}: {resource.Value}");
        }

        Console.WriteLine("\n--- Survivors ---");
        foreach (var s in Survivors)
        {
            Console.WriteLine($"ID: {s.Id} | {s.Name,-15} | HP: {s.Health,3}/100 | Morale: {s.Morale,3}/100 | Skill: {s.Skill,-10} | Task: {s.CurrentTask}");
        }
        
        Console.WriteLine("\n--- Buildings ---");
        if (Buildings.Count == 0)
        {
            Console.WriteLine("No buildings constructed.");
        }
        else
        {
            foreach (var b in Buildings)
            {
                Console.WriteLine($"{b.Type}: {(b.IsComplete ? "Complete" : $"In Progress ({b.BuildProgress}/{b.RequiredProgress})")}");
            }
        }
        Console.WriteLine("----------------------------------------");
    }

    private void HandlePlayerInput()
    {
        Console.WriteLine("\nEnter commands for the day. Type 'next' to end the day.");
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(input)) continue;

            if (input == "next") break;

            var parts = input.Split(' ');
            var command = parts[0];

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "assign":
                    AssignTask(parts);
                    break;
                case "build":
                    BuildStructure(parts);
                    break;
                case "map":
                    DisplayMap();
                    break;
                case "quit":
                    _gameOver = true;
                    return;
                default:
                    Console.WriteLine("Unknown command. Type 'help' for a list of commands.");
                    break;
            }
        }
    }
    
    private void AssignTask(string[] parts)
    {
        if (parts.Length < 3)
        {
            Console.WriteLine("Usage: assign <survivor_id> <task>");
            Console.WriteLine("Tasks: idle, scavenging, building, farming, resting");
            return;
        }

        if (!int.TryParse(parts[1], out int survivorId))
        {
            Console.WriteLine("Invalid survivor ID.");
            return;
        }

        var survivor = Survivors.FirstOrDefault(s => s.Id == survivorId);
        if (survivor == null)
        {
            Console.WriteLine($"No survivor with ID {survivorId} found.");
            return;
        }

        if (!Enum.TryParse<SurvivorTask>(parts[2], true, out var task))
        {
            Console.WriteLine($"Invalid task '{parts[2]}'.");
            return;
        }

        survivor.CurrentTask = task;
        Console.WriteLine($"{survivor.Name} assigned to {task}.");
    }
    
    private void BuildStructure(string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("Usage: build <building_type>");
            Console.WriteLine("Types: shelter, farm, waterpurifier, infirmary");
            return;
        }

        if (!Enum.TryParse<BuildingType>(parts[1], true, out var buildingType))
        {
            Console.WriteLine($"Invalid building type '{parts[1]}'.");
            return;
        }

        if (Buildings.Any(b => b.Type == buildingType))
        {
            Console.WriteLine($"A {buildingType} is already under construction or completed.");
            return;
        }

        int requiredMaterials = buildingType switch
        {
            BuildingType.Shelter => 20,
            BuildingType.Farm => 30,
            BuildingType.WaterPurifier => 25,
            BuildingType.Infirmary => 40,
            _ => 100
        };

        if (Resources[ResourceType.BuildingMaterials] < requiredMaterials)
        {
            Console.WriteLine($"Not enough Building Materials. Requires {requiredMaterials}, have {Resources[ResourceType.BuildingMaterials]}.");
            return;
        }

        Resources[ResourceType.BuildingMaterials] -= requiredMaterials;
        var newBuilding = new Building(buildingType);
        Buildings.Add(newBuilding);
        Console.WriteLine($"Started construction on {buildingType}. It will cost {requiredMaterials} building materials. Assign survivors to 'building' to make progress.");
    }
    
    private void ShowHelp()
    {
        Console.WriteLine("\n--- Available Commands ---");
        Console.WriteLine("assign <id> <task>   - Assign a survivor to a task (e.g., 'assign 1 scavenging').");
        Console.WriteLine("  Tasks: idle, scavenging, building, farming, resting");
        Console.WriteLine("build <type>         - Start construction of a new building (e.g., 'build farm').");
        Console.WriteLine("  Types: shelter, farm, waterpurifier, infirmary");
        Console.WriteLine("map                  - Display the world map.");
        Console.WriteLine("status               - Display the current status of the colony (shown automatically).");
        Console.WriteLine("next                 - End the current day and process all actions.");
        Console.WriteLine("quit                 - End the game.");
        Console.WriteLine("-------------------------");
    }

    private void DisplayMap()
    {
        Console.WriteLine("\n--- World Map ---");
        var sb = new StringBuilder();
        sb.AppendLine("  0 1 2 3 4 5 6 7 8 9");
        for (int y = 0; y < MapHeight; y++)
        {
            sb.Append($"{y} ");
            for (int x = 0; x < MapWidth; x++)
            {
                var tile = Map[x, y];
                char symbol = '?';
                if (tile.IsExplored || (x == MapWidth / 2 && y == MapHeight / 2))
                {
                    symbol = tile.Biome switch
                    {
                        BiomeType.CrashSite => 'C',
                        BiomeType.Forest => 'F',
                        BiomeType.Mountains => 'M',
                        BiomeType.Plains => 'P',
                        BiomeType.Ruins => 'R',
                        _ => '?'
                    };
                }
                sb.Append(symbol + " ");
            }
            sb.AppendLine();
        }
        sb.AppendLine("Legend: ?=Unexplored, C=CrashSite, F=Forest, M=Mountains, P=Plains, R=Ruins");
        Console.WriteLine(sb.ToString());
    }

    private void AdvanceDay()
    {
        Console.WriteLine($"\n--- Processing Day {Day} ---");
        
        EventBonuses.Clear();
        EventPenalties.Clear();

        // 1. Task Phase
        ProcessTasks();

        // 2. Consumption Phase
        int foodConsumed = Survivors.Count;
        int waterConsumed = Survivors.Count;
        Resources[ResourceType.Food] -= foodConsumed;
        Resources[ResourceType.Water] -= waterConsumed;
        Console.WriteLine($"Colony consumed {foodConsumed} food and {waterConsumed} water.");

        if (Resources[ResourceType.Food] < 0)
        {
            Console.WriteLine("Not enough food! Survivors are starving.");
            foreach (var s in Survivors) { s.Health -= 10; s.Morale -= 10; }
            Resources[ResourceType.Food] = 0;
        }
        if (Resources[ResourceType.Water] < 0)
        {
            Console.WriteLine("Not enough water! Survivors are dehydrated.");
            foreach (var s in Survivors) { s.Health -= 15; s.Morale -= 5; }
            Resources[ResourceType.Water] = 0;
        }

        // 3. Update Building effects
        ApplyBuildingEffects();
        
        // 4. Random Event Phase
        _eventManager.TriggerRandomEvent(this);
        
        // 5. Survivor stat decay/recovery
        foreach (var s in Survivors)
        {
            if (s.Health <= 0) s.Health = 0;
            if (s.Health > 100) s.Health = 100;
            if (s.Morale <= 0) s.Morale = 0;
            if (s.Morale > 100) s.Morale = 100;
        }
        
        Day++;
    }

    private void ProcessTasks()
    {
        var crashSiteTile = Map[MapWidth / 2, MapHeight / 2];
        foreach (var survivor in Survivors)
        {
            int baseYield = 5;
            switch (survivor.CurrentTask)
            {
                case SurvivorTask.Scavenging:
                    int scavengeBonus = survivor.Skill == SurvivorSkill.Scavenger ? 3 : 0;
                    scavengeBonus += EventBonuses.GetValueOrDefault("Scavenging", 0);
                    int materialsFound = baseYield + scavengeBonus;
                    if (crashSiteTile.Resources[ResourceType.BuildingMaterials] > 0)
                    {
                        int taken = Math.Min(materialsFound, crashSiteTile.Resources[ResourceType.BuildingMaterials]);
                        Resources[ResourceType.BuildingMaterials] += taken;
                        crashSiteTile.Resources[ResourceType.BuildingMaterials] -= taken;
                        Console.WriteLine($"{survivor.Name} scavenged {taken} Building Materials.");
                    }
                    else
                    {
                        Console.WriteLine($"{survivor.Name} scavenged but found no more materials at the crash site.");
                    }
                    break;
                case SurvivorTask.Building:
                    var project = Buildings.FirstOrDefault(b => !b.IsComplete);
                    if (project != null)
                    {
                        int buildBonus = survivor.Skill == SurvivorSkill.Builder ? 3 : 0;
                        buildBonus -= EventPenalties.GetValueOrDefault("Building", 0);
                        int progress = baseYield + buildBonus;
                        project.BuildProgress += progress;
                        Console.WriteLine($"{survivor.Name} worked on the {project.Type}, adding {progress} progress.");
                        if (project.IsComplete)
                        {
                            Console.WriteLine($"Construction of the {project.Type} is complete!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{survivor.Name} is ready to build, but there are no construction projects.");
                    }
                    break;
                case SurvivorTask.Farming:
                    var farm = Buildings.FirstOrDefault(b => b.Type == BuildingType.Farm && b.IsComplete);
                    if(farm != null)
                    {
                        int farmBonus = survivor.Skill == SurvivorSkill.Farmer ? 3 : 0;
                        farmBonus += EventBonuses.GetValueOrDefault("Farming", 0);
                        int foodFarmed = baseYield + farmBonus;
                        Resources[ResourceType.Food] += foodFarmed;
                        Console.WriteLine($"{survivor.Name} farmed {foodFarmed} food.");
                    }
                    else
                    {
                         Console.WriteLine($"{survivor.Name} is assigned to farming, but no farm is built.");
                    }
                    break;
                case SurvivorTask.Resting:
                    int healthGain = 10;
                    int moraleGain = 10;
                    survivor.Health += healthGain;
                    survivor.Morale += moraleGain;
                    Console.WriteLine($"{survivor.Name} is resting, recovering {healthGain} health and {moraleGain} morale.");
                    break;
            }
        }
    }
    
    private void ApplyBuildingEffects()
    {
        var shelter = Buildings.FirstOrDefault(b => b.Type == BuildingType.Shelter && b.IsComplete);
        if (shelter != null)
        {
            foreach (var survivor in Survivors)
            {
                survivor.Morale += 5; // Passive morale boost from shelter
            }
        }

        var waterPurifier = Buildings.FirstOrDefault(b => b.Type == BuildingType.WaterPurifier && b.IsComplete);
        if (waterPurifier != null)
        {
            int waterProduced = 10;
            Resources[ResourceType.Water] += waterProduced;
            Console.WriteLine($"The Water Purifier produced {waterProduced} water.");
        }
    }

    private void CheckEndConditions()
    {
        Survivors.RemoveAll(s => s.Health <= 0);
        if (!Survivors.Any())
        {
            Console.WriteLine("All of your survivors have perished. The colony is lost.");
            _gameOver = true;
        }

        if (Day > 100)
        {
            Console.WriteLine("You have survived for 100 days and established a foothold on this new world. You win!");
            _gameOver = true;
        }
    }
}