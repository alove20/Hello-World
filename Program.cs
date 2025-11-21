using System;
using System.Collections.Generic;
using System.Linq;

namespace ColonyShipExodus
{
    // Enums for types
    enum ResourceType { Food, Water, Medicine, BuildingMaterials, Energy }
    enum SurvivorSkill { Scavenging, Building, Farming, Resting, Exploring }
    enum BiomeType { CrashSite, Forest, Mine, Ruins, Desert }
    enum BuildingType { Shelter, Farm, WaterPurifier, Infirmary, Generator }

    // Classes
    class Resource
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }

        public Resource(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }

    class Survivor
    {
        public string Name { get; set; }
        public SurvivorSkill Skill { get; set; }
        public int Health { get; set; } // 0-100
        public int Morale { get; set; } // 0-100

        public Survivor(string name, SurvivorSkill skill)
        {
            Name = name;
            Skill = skill;
            Health = 100;
            Morale = 100;
        }
    }

    class Building
    {
        public BuildingType Type { get; set; }
        public bool IsBuilt { get; set; }
        public int Progress { get; set; } // 0-100

        public Building(BuildingType type)
        {
            Type = type;
            IsBuilt = false;
            Progress = 0;
        }
    }

    class MapTile
    {
        public BiomeType Biome { get; set; }
        public bool Explored { get; set; }
        public List<Resource> Resources { get; set; }

        public MapTile(BiomeType biome)
        {
            Biome = biome;
            Explored = false;
            Resources = new List<Resource>();
        }
    }

    class Player
    {
        public List<Survivor> Survivors { get; set; }
        public Dictionary<ResourceType, int> Inventory { get; set; }
        public List<Building> Buildings { get; set; }
        public MapTile[,] Map { get; set; }
        public int Day { get; set; }

        public Player()
        {
            Survivors = new List<Survivor>();
            Inventory = new Dictionary<ResourceType, int>();
            Buildings = new List<Building>();
            Map = new MapTile[10, 10]; // 10x10 grid
            Day = 1;
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Add initial survivors
            Survivors.Add(new Survivor("Alice", SurvivorSkill.Scavenging));
            Survivors.Add(new Survivor("Bob", SurvivorSkill.Building));
            Survivors.Add(new Survivor("Charlie", SurvivorSkill.Farming));

            // Initialize inventory
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                Inventory[type] = 10; // Starting amount
            }

            // Generate map
            Random rand = new Random();
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    BiomeType biome = (BiomeType)rand.Next(Enum.GetValues(typeof(BiomeType)).Length);
                    Map[x, y] = new MapTile(biome);
                    // Add some resources based on biome
                    if (biome == BiomeType.Forest) Map[x, y].Resources.Add(new Resource(ResourceType.Food, rand.Next(5, 15)));
                    else if (biome == BiomeType.Mine) Map[x, y].Resources.Add(new Resource(ResourceType.BuildingMaterials, rand.Next(10, 20)));
                    // Etc.
                }
            }

            // Mark crash site as explored
            Map[5, 5].Explored = true;
        }
    }

    class Game
    {
        private Player player;
        private Random rand;

        public Game()
        {
            player = new Player();
            rand = new Random();
        }

        public void Run()
        {
            Console.WriteLine("Welcome to Colony Ship Exodus!");
            while (true)
            {
                DisplayStatus();
                string input = GetPlayerInput();
                ProcessTurn(input);
                if (CheckWinLoss()) break;
            }
        }

        private void DisplayStatus()
        {
            Console.WriteLine($"\nDay {player.Day}");
            Console.WriteLine("Survivors:");
            foreach (var survivor in player.Survivors)
            {
                Console.WriteLine($"{survivor.Name}: Skill {survivor.Skill}, Health {survivor.Health}, Morale {survivor.Morale}");
            }
            Console.WriteLine("Inventory:");
            foreach (var kvp in player.Inventory)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
            Console.WriteLine("Buildings:");
            foreach (var building in player.Buildings)
            {
                Console.WriteLine($"{building.Type}: {(building.IsBuilt ? "Built" : $"Progress {building.Progress}%")}");
            }
        }

        private string GetPlayerInput()
        {
            Console.WriteLine("\nActions: explore, scavenge, build, rest, event");
            Console.Write("Choose action: ");
            return Console.ReadLine().ToLower();
        }

        private void ProcessTurn(string action)
        {
            switch (action)
            {
                case "explore":
                    Explore();
                    break;
                case "scavenge":
                    Scavenge();
                    break;
                case "build":
                    Build();
                    break;
                case "rest":
                    Rest();
                    break;
                case "event":
                    HandleEvent();
                    break;
                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
            // Consume resources
            ConsumeResources();
            player.Day++;
        }

        private void Explore()
        {
            // Simple exploration: reveal a random tile
            int x = rand.Next(10);
            int y = rand.Next(10);
            if (!player.Map[x, y].Explored)
            {
                player.Map[x, y].Explored = true;
                Console.WriteLine($"Explored {player.Map[x, y].Biome} at ({x},{y})");
            }
            else
            {
                Console.WriteLine("Already explored.");
            }
        }

        private void Scavenge()
        {
            // Gather resources from explored tiles
            foreach (var tile in player.Map)
            {
                if (tile.Explored)
                {
                    foreach (var res in tile.Resources)
                    {
                        player.Inventory[res.Type] += res.Amount;
                    }
                    tile.Resources.Clear(); // Deplete
                }
            }
            Console.WriteLine("Scavenged resources.");
        }

        private void Build()
        {
            Console.Write("Build what? (shelter, farm, etc.): ");
            string build = Console.ReadLine().ToLower();
            BuildingType type;
            if (Enum.TryParse(build, true, out type))
            {
                var building = player.Buildings.FirstOrDefault(b => b.Type == type);
                if (building == null)
                {
                    building = new Building(type);
                    player.Buildings.Add(building);
                }
                if (!building.IsBuilt && player.Inventory[ResourceType.BuildingMaterials] >= 10)
                {
                    building.Progress += 25; // Arbitrary progress
                    player.Inventory[ResourceType.BuildingMaterials] -= 10;
                    if (building.Progress >= 100)
                    {
                        building.IsBuilt = true;
                        Console.WriteLine($"{type} built!");
                    }
                }
                else
                {
                    Console.WriteLine("Not enough materials or already built.");
                }
            }
        }

        private void Rest()
        {
            foreach (var survivor in player.Survivors)
            {
                survivor.Health = Math.Min(100, survivor.Health + 10);
                survivor.Morale = Math.Min(100, survivor.Morale + 5);
            }
            Console.WriteLine("Survivors rested.");
        }

        private void HandleEvent()
        {
            int eventType = rand.Next(3);
            if (eventType == 0)
            {
                Console.WriteLine("Alien encounter! Lost some food.");
                player.Inventory[ResourceType.Food] -= 5;
            }
            else if (eventType == 1)
            {
                Console.WriteLine("Disease outbreak! Health decreased.");
                foreach (var survivor in player.Survivors)
                {
                    survivor.Health -= 10;
                }
            }
            else
            {
                Console.WriteLine("Found rare resources! Gained building materials.");
                player.Inventory[ResourceType.BuildingMaterials] += 10;
            }
        }

        private void ConsumeResources()
        {
            player.Inventory[ResourceType.Food] -= player.Survivors.Count;
            player.Inventory[ResourceType.Water] -= player.Survivors.Count;
            if (player.Inventory[ResourceType.Food] < 0) player.Inventory[ResourceType.Food] = 0;
            if (player.Inventory[ResourceType.Water] < 0) player.Inventory[ResourceType.Water] = 0;
        }

        private bool CheckWinLoss()
        {
            if (player.Inventory[ResourceType.Food] <= 0 || player.Inventory[ResourceType.Water] <= 0)
            {
                Console.WriteLine("Game Over: Resources depleted.");
                return true;
            }
            if (player.Buildings.Count(b => b.IsBuilt) >= 3)
            {
                Console.WriteLine("You Win: Colony established!");
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}