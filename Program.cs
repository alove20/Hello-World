using System;
using System.Collections.Generic;
using System.Linq;

namespace ColonyShipExodus
{
    // Enums for game elements
    enum ResourceType { Food, Water, Medicine, BuildingMaterials, Energy }
    enum SurvivorSkill { Scavenging, Building, Farming, Resting }
    enum BiomeType { Forest, Mine, Ruin, Plains, CrashSite }
    enum EventType { AlienEncounter, DiseaseOutbreak, GoodWeather, ResourceDiscovery, None }

    // Survivor class
    class Survivor
    {
        public string Name { get; set; }
        public int Health { get; set; } = 100;
        public int Morale { get; set; } = 50;
        public SurvivorSkill Skill { get; set; }

        public Survivor(string name, SurvivorSkill skill)
        {
            Name = name;
            Skill = skill;
        }

        public void AssignTask(SurvivorSkill task)
        {
            Skill = task;
        }

        public string GetStatus()
        {
            return $"{Name} (Health: {Health}, Morale: {Morale}, Skill: {Skill})";
        }
    }

    // Resource class
    class Resource
    {
        public ResourceType Type { get; set; }
        public int Quantity { get; set; }

        public Resource(ResourceType type, int quantity)
        {
            Type = type;
            Quantity = quantity;
        }

        public void AdjustQuantity(int amount)
        {
            Quantity = Math.Max(0, Quantity + amount);
        }
    }

    // MapTile class for world generation
    class MapTile
    {
        public BiomeType Biome { get; set; }
        public bool Explored { get; set; } = false;
        public List<Resource> AvailableResources { get; set; } = new List<Resource>();

        public MapTile(BiomeType biome)
        {
            Biome = biome;
            GenerateResources();
        }

        private void GenerateResources()
        {
            Random rand = new Random();
            switch (Biome)
            {
                case BiomeType.Forest:
                    AvailableResources.Add(new Resource(ResourceType.Food, rand.Next(5, 15)));
                    AvailableResources.Add(new Resource(ResourceType.BuildingMaterials, rand.Next(3, 10)));
                    break;
                case BiomeType.Mine:
                    AvailableResources.Add(new Resource(ResourceType.BuildingMaterials, rand.Next(10, 20)));
                    AvailableResources.Add(new Resource(ResourceType.Energy, rand.Next(5, 10)));
                    break;
                case BiomeType.Ruin:
                    AvailableResources.Add(new Resource(ResourceType.Medicine, rand.Next(2, 8)));
                    AvailableResources.Add(new Resource(ResourceType.BuildingMaterials, rand.Next(5, 15)));
                    break;
                case BiomeType.Plains:
                    AvailableResources.Add(new Resource(ResourceType.Food, rand.Next(3, 10)));
                    AvailableResources.Add(new Resource(ResourceType.Water, rand.Next(5, 15)));
                    break;
                case BiomeType.CrashSite:
                    AvailableResources.Add(new Resource(ResourceType.Energy, rand.Next(10, 20)));
                    AvailableResources.Add(new Resource(ResourceType.BuildingMaterials, rand.Next(5, 10)));
                    break;
            }
        }
    }

    // Building class
    class Building
    {
        public string Name { get; set; }
        public bool Constructed { get; set; } = false;
        public Dictionary<ResourceType, int> Cost { get; set; } = new Dictionary<ResourceType, int>();
        public string Effect { get; set; }

        public Building(string name, string effect)
        {
            Name = name;
            Effect = effect;
            // Default costs - can be customized
            Cost[ResourceType.BuildingMaterials] = 10;
            Cost[ResourceType.Energy] = 5;
        }

        public bool CanBuild(Dictionary<ResourceType, Resource> inventory)
        {
            foreach (var kvp in Cost)
            {
                if (!inventory.ContainsKey(kvp.Key) || inventory[kvp.Key].Quantity < kvp.Value)
                    return false;
            }
            return true;
        }

        public void Build(Dictionary<ResourceType, Resource> inventory)
        {
            foreach (var kvp in Cost)
            {
                inventory[kvp.Key].AdjustQuantity(-kvp.Value);
            }
            Constructed = true;
        }
    }

    // Game class to manage state
    class Game
    {
        public int Day { get; set; } = 1;
        public Dictionary<ResourceType, Resource> Inventory { get; set; } = new Dictionary<ResourceType, Resource>();
        public List<Survivor> Survivors { get; set; } = new List<Survivor>();
        public MapTile[,] Map { get; set; }
        public List<Building> Buildings { get; set; } = new List<Building>();
        public Random Rand { get; set; } = new Random();

        public Game(int mapSize = 5)
        {
            // Initialize inventory
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                Inventory[type] = new Resource(type, 50); // Starting amounts
            }

            // Initialize survivors
            Survivors.Add(new Survivor("Alice", SurvivorSkill.Scavenging));
            Survivors.Add(new Survivor("Bob", SurvivorSkill.Building));
            Survivors.Add(new Survivor("Charlie", SurvivorSkill.Farming));

            // Generate map
            Map = new MapTile[mapSize, mapSize];
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    BiomeType biome = (BiomeType)Rand.Next(Enum.GetValues(typeof(BiomeType)).Length);
                    Map[x, y] = new MapTile(biome);
                }
            }
            // Crash site at center
            Map[mapSize / 2, mapSize / 2] = new MapTile(BiomeType.CrashSite);
            Map[mapSize / 2, mapSize / 2].Explored = true;

            // Initialize buildings
            Buildings.Add(new Building("Shelter", "Increases morale and protects from events."));
            Buildings.Add(new Building("Farm", "Produces extra food."));
            Buildings.Add(new Building("Water Purifier", "Produces extra water."));
            Buildings.Add(new Building("Infirmary", "Improves health recovery."));
        }

        public void ProcessTurn()
        {
            Day++;

            // Consume resources
            Inventory[ResourceType.Food].AdjustQuantity(-Survivors.Count * 2);
            Inventory[ResourceType.Water].AdjustQuantity(-Survivors.Count * 2);
            Inventory[ResourceType.Energy].AdjustQuantity(-5);

            // Process survivor tasks
            foreach (var survivor in Survivors)
            {
                switch (survivor.Skill)
                {
                    case SurvivorSkill.Scavenging:
                        // Explore a random unexplored tile
                        var unexplored = GetUnexploredTiles();
                        if (unexplored.Count > 0)
                        {
                            var tile = unexplored[Rand.Next(unexplored.Count)];
                            tile.Explored = true;
                            foreach (var res in tile.AvailableResources)
                            {
                                if (Inventory.ContainsKey(res.Type))
                                    Inventory[res.Type].AdjustQuantity(res.Quantity);
                            }
                            Console.WriteLine($"{survivor.Name} explored a {tile.Biome} tile and gathered resources!");
                        }
                        break;
                    case SurvivorSkill.Building:
                        // Attempt to build a building
                        var unbuilt = Buildings.Where(b => !b.Constructed).ToList();
                        if (unbuilt.Count > 0)
                        {
                            var building = unbuilt[Rand.Next(unbuilt.Count)];
                            if (building.CanBuild(Inventory))
                            {
                                building.Build(Inventory);
                                Console.WriteLine($"{survivor.Name} constructed {building.Name}!");
                            }
                        }
                        break;
                    case SurvivorSkill.Farming:
                        if (Buildings.Any(b => b.Name == "Farm" && b.Constructed))
                            Inventory[ResourceType.Food].AdjustQuantity(5);
                        break;
                    case SurvivorSkill.Resting:
                        survivor.Health = Math.Min(100, survivor.Health + 10);
                        survivor.Morale = Math.Min(100, survivor.Morale + 5);
                        break;
                }
                // General health/morale decay
                survivor.Health = Math.Max(0, survivor.Health - Rand.Next(0, 5));
                survivor.Morale = Math.Max(0, survivor.Morale - Rand.Next(0, 5));
            }

            // Process buildings
            if (Buildings.Any(b => b.Name == "Water Purifier" && b.Constructed))
                Inventory[ResourceType.Water].AdjustQuantity(5);
            if (Buildings.Any(b => b.Name == "Infirmary" && b.Constructed))
            {
                foreach (var s in Survivors)
                    s.Health = Math.Min(100, s.Health + 5);
            }

            // Random event
            EventType eventType = (EventType)Rand.Next(Enum.GetValues(typeof(EventType)).Length);
            switch (eventType)
            {
                case EventType.AlienEncounter:
                    Console.WriteLine("Alien encounter! Lost some resources.");
                    Inventory[ResourceType.Food].AdjustQuantity(-5);
                    break;
                case EventType.DiseaseOutbreak:
                    Console.WriteLine("Disease outbreak! Health decreased.");
                    foreach (var s in Survivors)
                        s.Health = Math.Max(0, s.Health - 10);
                    break;
                case EventType.GoodWeather:
                    Console.WriteLine("Good weather! Morale boosted.");
                    foreach (var s in Survivors)
                        s.Morale = Math.Min(100, s.Morale + 10);
                    break;
                case EventType.ResourceDiscovery:
                    Console.WriteLine("Resource discovery! Found extra materials.");
                    Inventory[ResourceType.BuildingMaterials].AdjustQuantity(10);
                    break;
                default:
                    break;
            }

            // Check win/lose conditions
            if (Survivors.All(s => s.Health <= 0))
            {
                Console.WriteLine("All survivors have died. Game over.");
                Environment.Exit(0);
            }
            if (Buildings.All(b => b.Constructed) && Survivors.All(s => s.Health >= 80 && s.Morale >= 80))
            {
                Console.WriteLine("Colony established! You win.");
                Environment.Exit(0);
            }
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"Day {Day}");
            Console.WriteLine("Resources:");
            foreach (var res in Inventory)
            {
                Console.WriteLine($"  {res.Key}: {res.Value.Quantity}");
            }
            Console.WriteLine("Survivors:");
            foreach (var s in Survivors)
            {
                Console.WriteLine($"  {s.GetStatus()}");
            }
            Console.WriteLine("Buildings:");
            foreach (var b in Buildings)
            {
                Console.WriteLine($"  {b.Name}: {(b.Constructed ? "Built" : "Not Built")}");
            }
        }

        public void AssignTasks()
        {
            foreach (var survivor in Survivors)
            {
                Console.WriteLine($"Assign task for {survivor.Name} (current: {survivor.Skill}):");
                Console.WriteLine("1. Scavenging 2. Building 3. Farming 4. Resting");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1": survivor.AssignTask(SurvivorSkill.Scavenging); break;
                    case "2": survivor.AssignTask(SurvivorSkill.Building); break;
                    case "3": survivor.AssignTask(SurvivorSkill.Farming); break;
                    case "4": survivor.AssignTask(SurvivorSkill.Resting); break;
                }
            }
        }

        private List<MapTile> GetUnexploredTiles()
        {
            List<MapTile> tiles = new List<MapTile>();
            for (int x = 0; x < Map.GetLength(0); x++)
            {
                for (int y = 0; y < Map.GetLength(1); y++)
                {
                    if (!Map[x, y].Explored) tiles.Add(Map[x, y]);
                }
            }
            return tiles;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Colony Ship Exodus!");
            Game game = new Game();

            while (true)
            {
                game.DisplayStatus();
                game.AssignTasks();
                Console.WriteLine("Press Enter to advance to next day...");
                Console.ReadLine();
                game.ProcessTurn();
            }
        }
    }
}