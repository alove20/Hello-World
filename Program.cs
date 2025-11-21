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
            Console.WriteLine("You are the commander of a crash-landed colony on an alien planet.");
            Console.WriteLine("Survive and build a new home.\n");

            Game game = new Game();
            game.Initialize();

            while (!game.IsGameOver)
            {
                game.DisplayStatus();
                string input = game.GetPlayerInput();
                game.ProcessTurn(input);
                game.CheckWinLossConditions();
            }

            game.DisplayEndGameMessage();
        }
    }

    public class Game
    {
        public bool IsGameOver { get; private set; } = false;
        public Player Player { get; private set; }
        public Map Map { get; private set; }
        public List<Event> Events { get; private set; }
        private Random random = new Random();

        public void Initialize()
        {
            Player = new Player();
            Map = new Map(10, 10); // 10x10 grid
            Map.Generate();
            Events = new List<Event>
            {
                new Event("Alien Encounter", "Aliens approach the camp. Lose 1 survivor if not prepared."),
                new Event("Disease Outbreak", "A disease spreads. Lose health in survivors."),
                new Event("Resource Cache", "Found a hidden cache. Gain random resources."),
                new Event("Weather Storm", "A storm hits. Reduce production temporarily.")
            };
        }

        public void DisplayStatus()
        {
            Console.WriteLine("\n--- Day " + Player.Day + " ---");
            Console.WriteLine("Survivors: " + Player.Survivors.Count + " (Health: " + string.Join(", ", Player.Survivors.Select(s => s.Health)) + ")");
            Console.WriteLine("Resources:");
            foreach (var res in Player.Resources)
            {
                Console.WriteLine($"  {res.Type}: {res.Amount}");
            }
            Console.WriteLine("Buildings: " + string.Join(", ", Player.Buildings.Select(b => b.Type.ToString())));
            Console.WriteLine("\nActions: explore, gather, build [type], assign [skill], rest, status");
        }

        public string GetPlayerInput()
        {
            Console.Write("Command: ");
            return Console.ReadLine().ToLower();
        }

        public void ProcessTurn(string input)
        {
            string[] parts = input.Split(' ');
            string command = parts[0];

            switch (command)
            {
                case "explore":
                    Explore();
                    break;
                case "gather":
                    GatherResources();
                    break;
                case "build":
                    if (parts.Length > 1 && Enum.TryParse<BuildingType>(parts[1], true, out BuildingType type))
                    {
                        Build(type);
                    }
                    else
                    {
                        Console.WriteLine("Invalid building type.");
                    }
                    break;
                case "assign":
                    if (parts.Length > 1 && Enum.TryParse<SurvivorSkill>(parts[1], true, out SurvivorSkill skill))
                    {
                        AssignSurvivors(skill);
                    }
                    else
                    {
                        Console.WriteLine("Invalid skill.");
                    }
                    break;
                case "rest":
                    RestSurvivors();
                    break;
                case "status":
                    // Already displayed
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }

            // Consume resources
            Player.ConsumeDailyResources();

            // Handle random event
            if (random.Next(100) < 30) // 30% chance
            {
                var evt = Events[random.Next(Events.Count)];
                HandleEvent(evt);
            }

            Player.Day++;
        }

        private void Explore()
        {
            // Simple exploration: reveal a random tile and gain minor resources
            var tile = Map.Tiles[random.Next(Map.Width), random.Next(Map.Height)];
            tile.Explored = true;
            Console.WriteLine($"Explored {tile.Biome}. Found some resources.");
            Player.Resources.First(r => r.Type == ResourceType.Food).Amount += 5;
            Player.Resources.First(r => r.Type == ResourceType.Water).Amount += 5;
        }

        private void GatherResources()
        {
            // Gather based on assigned survivors
            int gatherers = Player.Survivors.Count(s => s.AssignedSkill == SurvivorSkill.Scavenging);
            Player.Resources.First(r => r.Type == ResourceType.Food).Amount += gatherers * 10;
            Player.Resources.First(r => r.Type == ResourceType.BuildingMaterials).Amount += gatherers * 5;
            Console.WriteLine($"Gathered resources with {gatherers} scavengers.");
        }

        private void Build(BuildingType type)
        {
            var res = Player.Resources.FirstOrDefault(r => r.Type == ResourceType.BuildingMaterials);
            if (res != null && res.Amount >= 20 && !Player.Buildings.Any(b => b.Type == type))
            {
                res.Amount -= 20;
                Player.Buildings.Add(new Building(type));
                Console.WriteLine($"Built {type}.");
                // Unlock effects, e.g., farm increases food production
            }
            else
            {
                Console.WriteLine("Not enough materials or already built.");
            }
        }

        private void AssignSurvivors(SurvivorSkill skill)
        {
            foreach (var survivor in Player.Survivors)
            {
                survivor.AssignedSkill = skill;
            }
            Console.WriteLine($"Assigned all survivors to {skill}.");
        }

        private void RestSurvivors()
        {
            foreach (var survivor in Player.Survivors)
            {
                survivor.Health = Math.Min(100, survivor.Health + 10);
            }
            Console.WriteLine("Survivors rested and regained health.");
        }

        private void HandleEvent(Event evt)
        {
            Console.WriteLine($"Event: {evt.Description}");
            // Simple effects
            if (evt.Name == "Alien Encounter")
            {
                if (Player.Survivors.Count > 1) Player.Survivors.RemoveAt(0);
            }
            else if (evt.Name == "Disease Outbreak")
            {
                foreach (var s in Player.Survivors) s.Health -= 20;
            }
            else if (evt.Name == "Resource Cache")
            {
                Player.Resources.First(r => r.Type == ResourceType.Medicine).Amount += 10;
            }
            else if (evt.Name == "Weather Storm")
            {
                // Temporary, no change in this simple version
            }
        }

        public void CheckWinLossConditions()
        {
            if (Player.Survivors.Count == 0)
            {
                IsGameOver = true;
                Console.WriteLine("All survivors perished. Game over.");
            }
            else if (Player.Day > 30 && Player.Buildings.Count >= 3) // Win after 30 days with 3 buildings
            {
                IsGameOver = true;
                Console.WriteLine("Colony established! You win.");
            }
        }

        public void DisplayEndGameMessage()
        {
            Console.WriteLine("Thanks for playing Colony Ship Exodus!");
        }
    }

    public class Player
    {
        public int Day { get; set; } = 1;
        public List<Survivor> Survivors { get; set; } = new List<Survivor>
        {
            new Survivor("Alice", SurvivorSkill.Scavenging),
            new Survivor("Bob", SurvivorSkill.Building),
            new Survivor("Charlie", SurvivorSkill.Farming)
        };
        public List<Resource> Resources { get; set; } = new List<Resource>
        {
            new Resource(ResourceType.Food, 50),
            new Resource(ResourceType.Water, 50),
            new Resource(ResourceType.Medicine, 10),
            new Resource(ResourceType.BuildingMaterials, 20),
            new Resource(ResourceType.Energy, 100)
        };
        public List<Building> Buildings { get; set; } = new List<Building>();

        public void ConsumeDailyResources()
        {
            Resources.First(r => r.Type == ResourceType.Food).Amount -= Survivors.Count * 2;
            Resources.First(r => r.Type == ResourceType.Water).Amount -= Survivors.Count * 2;
            // If negative, survivors lose health
            if (Resources.First(r => r.Type == ResourceType.Food).Amount < 0)
            {
                foreach (var s in Survivors) s.Health -= 10;
                Resources.First(r => r.Type == ResourceType.Food).Amount = 0;
            }
            // Similar for water
        }
    }

    public class Survivor
    {
        public string Name { get; set; }
        public int Health { get; set; } = 100;
        public SurvivorSkill Skill { get; set; }
        public SurvivorSkill AssignedSkill { get; set; }

        public Survivor(string name, SurvivorSkill skill)
        {
            Name = name;
            Skill = skill;
            AssignedSkill = skill;
        }
    }

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

    public class Building
    {
        public BuildingType Type { get; set; }

        public Building(BuildingType type)
        {
            Type = type;
        }
    }

    public class Map
    {
        public int Width { get; }
        public int Height { get; }
        public MapTile[,] Tiles { get; }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new MapTile[width, height];
        }

        public void Generate()
        {
            Random random = new Random();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    BiomeType biome = (BiomeType)random.Next(Enum.GetValues(typeof(BiomeType)).Length);
                    Tiles[x, y] = new MapTile(biome);
                }
            }
        }
    }

    public class MapTile
    {
        public BiomeType Biome { get; set; }
        public bool Explored { get; set; } = false;

        public MapTile(BiomeType biome)
        {
            Biome = biome;
        }
    }

    public class Event
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Event(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public enum ResourceType { Food, Water, Medicine, BuildingMaterials, Energy }
    public enum SurvivorSkill { Scavenging, Building, Farming, Resting }
    public enum BuildingType { Shelter, Farm, WaterPurifier, Infirmary }
    public enum BiomeType { Forest, Mine, Ruins, Desert, Plains }
}