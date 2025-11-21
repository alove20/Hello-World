using System;
using System.Collections.Generic;
using System.Linq;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Colony Ship Exodus!");
            Console.WriteLine("You are the commander of a colony ship that has crash-landed on an alien planet.");
            Console.WriteLine("Your goal is to manage survivors, gather resources, and build a self-sufficient colony.");
            Console.WriteLine();

            Game game = new Game();
            game.Start();

            while (!game.IsGameOver)
            {
                game.DisplayStatus();
                game.ProcessTurn();
            }

            Console.WriteLine("Game Over! Thank you for playing.");
        }
    }

    public enum ResourceType { Food, Water, Medicine, BuildingMaterials, Energy }
    public enum SurvivorSkill { Scavenging, Building, Farming, Medical, Resting }
    public enum BiomeType { Forest, Mine, Ruins, Desert }
    public enum BuildingType { Shelter, Farm, WaterPurifier, Infirmary }

    public class Resource
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }

        public Resource(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }

    public class Survivor
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

        public void AssignTask(SurvivorSkill task)
        {
            Skill = task;
        }

        public void UpdateStatus()
        {
            // Simulate daily changes based on task
            switch (Skill)
            {
                case SurvivorSkill.Scavenging:
                    Morale -= 5; // Dangerous task
                    break;
                case SurvivorSkill.Building:
                    Health -= 2; // Physically demanding
                    break;
                case SurvivorSkill.Farming:
                    Morale += 5; // Rewarding
                    break;
                case SurvivorSkill.Medical:
                    Health += 10; // Healing others helps self
                    break;
                case SurvivorSkill.Resting:
                    Health += 5;
                    Morale += 5;
                    break;
            }

            // Clamp values
            Health = Math.Max(0, Math.Min(100, Health));
            Morale = Math.Max(0, Math.Min(100, Morale));
        }
    }

    public class Building
    {
        public BuildingType Type { get; set; }
        public bool IsBuilt { get; set; }
        public int BuildProgress { get; set; } // 0-100

        public Building(BuildingType type)
        {
            Type = type;
            IsBuilt = false;
            BuildProgress = 0;
        }

        public bool TryBuild(Dictionary<ResourceType, int> resources)
        {
            // Cost to build (simplified)
            var cost = new Dictionary<ResourceType, int>
            {
                { ResourceType.BuildingMaterials, 50 },
                { ResourceType.Energy, 20 }
            };

            if (cost.All(c => resources.ContainsKey(c.Key) && resources[c.Key] >= c.Value))
            {
                foreach (var c in cost)
                {
                    resources[c.Key] -= c.Value;
                }
                BuildProgress += 20; // Assume daily progress
                if (BuildProgress >= 100)
                {
                    IsBuilt = true;
                    Console.WriteLine($"{Type} has been completed!");
                }
                return true;
            }
            return false;
        }
    }

    public class MapTile
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

        public void GenerateResources(Random rand)
        {
            // Procedural resource generation based on biome
            switch (Biome)
            {
                case BiomeType.Forest:
                    Resources.Add(new Resource(ResourceType.Food, rand.Next(10, 20)));
                    break;
                case BiomeType.Mine:
                    Resources.Add(new Resource(ResourceType.BuildingMaterials, rand.Next(15, 30)));
                    break;
                case BiomeType.Ruins:
                    Resources.Add(new Resource(ResourceType.Energy, rand.Next(5, 15)));
                    break;
                case BiomeType.Desert:
                    Resources.Add(new Resource(ResourceType.Water, rand.Next(5, 15)));
                    break;
            }
        }
    }

    public class Game
    {
        public bool IsGameOver { get; private set; }
        public int Day { get; private set; }
        public Dictionary<ResourceType, int> Resources { get; private set; }
        public List<Survivor> Survivors { get; private set; }
        public MapTile[,] Map { get; private set; }
        public List<Building> Buildings { get; private set; }
        private Random rand;

        public Game()
        {
            IsGameOver = false;
            Day = 1;
            Resources = new Dictionary<ResourceType, int>
            {
                { ResourceType.Food, 100 },
                { ResourceType.Water, 100 },
                { ResourceType.Medicine, 50 },
                { ResourceType.BuildingMaterials, 0 },
                { ResourceType.Energy, 50 }
            };
            Survivors = new List<Survivor>
            {
                new Survivor("Alice", SurvivorSkill.Scavenging),
                new Survivor("Bob", SurvivorSkill.Building),
                new Survivor("Charlie", SurvivorSkill.Farming)
            };
            Buildings = new List<Building>
            {
                new Building(BuildingType.Shelter),
                new Building(BuildingType.Farm),
                new Building(BuildingType.WaterPurifier),
                new Building(BuildingType.Infirmary)
            };
            rand = new Random();
            GenerateMap();
        }

        private void GenerateMap()
        {
            Map = new MapTile[10, 10]; // 10x10 grid
            BiomeType[] biomes = { BiomeType.Forest, BiomeType.Mine, BiomeType.Ruins, BiomeType.Desert };
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Map[x, y] = new MapTile(biomes[rand.Next(biomes.Length)]);
                    Map[x, y].GenerateResources(rand);
                }
            }
        }

        public void Start()
        {
            Console.WriteLine("The colony ship has crash-landed. Survivors are ready for your commands.");
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"\n--- Day {Day} ---");
            Console.WriteLine("Resources:");
            foreach (var res in Resources)
            {
                Console.WriteLine($"{res.Key}: {res.Value}");
            }
            Console.WriteLine("\nSurvivors:");
            foreach (var surv in Survivors)
            {
                Console.WriteLine($"{surv.Name} - Skill: {surv.Skill}, Health: {surv.Health}, Morale: {surv.Morale}");
            }
            Console.WriteLine("\nBuildings:");
            foreach (var build in Buildings)
            {
                Console.WriteLine($"{build.Type}: {(build.IsBuilt ? "Built" : $"Progress: {build.BuildProgress}%")}");
            }
        }

        public void ProcessTurn()
        {
            Console.WriteLine("\nWhat would you like to do? (1: Assign tasks, 2: Build, 3: Explore, 4: Rest all, 5: End turn)");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AssignTasks();
                    break;
                case "2":
                    BuildStructure();
                    break;
                case "3":
                    Explore();
                    break;
                case "4":
                    RestAll();
                    break;
                case "5":
                    EndTurn();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        private void AssignTasks()
        {
            for (int i = 0; i < Survivors.Count; i++)
            {
                Console.WriteLine($"Assign task for {Survivors[i].Name} (Scavenging, Building, Farming, Medical, Resting):");
                string taskStr = Console.ReadLine();
                if (Enum.TryParse(taskStr, out SurvivorSkill skill))
                {
                    Survivors[i].AssignTask(skill);
                }
                else
                {
                    Console.WriteLine("Invalid skill. Defaulting to Resting.");
                    Survivors[i].AssignTask(SurvivorSkill.Resting);
                }
            }
        }

        private void BuildStructure()
        {
            Console.WriteLine("Choose building to work on:");
            for (int i = 0; i < Buildings.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {Buildings[i].Type}");
            }
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= Buildings.Count)
            {
                if (!Buildings[choice - 1].TryBuild(Resources))
                {
                    Console.WriteLine("Not enough resources.");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }

        private void Explore()
        {
            Console.WriteLine("Exploring a random tile...");
            int x = rand.Next(10), y = rand.Next(10);
            MapTile tile = Map[x, y];
            if (!tile.Explored)
            {
                tile.Explored = true;
                foreach (var res in tile.Resources)
                {
                    if (Resources.ContainsKey(res.Type))
                        Resources[res.Type] += res.Amount;
                }
                Console.WriteLine($"Discovered {tile.Biome} biome. Gained resources!");
            }
            else
            {
                Console.WriteLine("Tile already explored.");
            }
        }

        private void RestAll()
        {
            foreach (var surv in Survivors)
            {
                surv.AssignTask(SurvivorSkill.Resting);
            }
            Console.WriteLine("All survivors are resting.");
        }

        private void EndTurn()
        {
            // Process daily updates
            foreach (var surv in Survivors)
            {
                surv.UpdateStatus();
            }

            // Resource consumption
            Resources[ResourceType.Food] -= Survivors.Count * 5;
            Resources[ResourceType.Water] -= Survivors.Count * 3;

            // Random event
            if (rand.Next(10) < 3) // 30% chance
            {
                HandleRandomEvent();
            }

            // Check game over
            if (Resources[ResourceType.Food] <= 0 || Survivors.All(s => s.Health <= 0))
            {
                IsGameOver = true;
            }

            Day++;
        }

        private void HandleRandomEvent()
        {
            string[] events = { "Disease outbreak: Medicine -10", "Alien encounter: Morale -20", "Rare find: Energy +20" };
            string eventMsg = events[rand.Next(events.Length)];
            Console.WriteLine($"Random Event: {eventMsg}");

            if (eventMsg.Contains("Disease"))
            {
                Resources[ResourceType.Medicine] = Math.Max(0, Resources[ResourceType.Medicine] - 10);
            }
            else if (eventMsg.Contains("Alien"))
            {
                foreach (var surv in Survivors)
                {
                    surv.Morale -= 20;
                }
            }
            else if (eventMsg.Contains("Rare"))
            {
                Resources[ResourceType.Energy] += 20;
            }
        }
    }
}