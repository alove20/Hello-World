using System;
using System.Collections.Generic;
using System.Linq;

namespace ColonyShipExodus
{
    public class Game
    {
        private Player player;
        private Map map;
        private List<Survivor> survivors;
        private Dictionary<ResourceType, int> resources;
        private List<Building> buildings;
        private Random random;
        private int day;

        public Game()
        {
            random = new Random();
            day = 1;
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Initialize player
            player = new Player();

            // Generate map (simple 10x10 grid for demo)
            map = new Map(10, 10);
            map.Generate();

            // Initialize survivors (5 survivors)
            survivors = new List<Survivor>();
            for (int i = 0; i < 5; i++)
            {
                survivors.Add(new Survivor($"Survivor {i + 1}", random.Next(50, 100), random.Next(50, 100)));
            }

            // Initialize resources
            resources = new Dictionary<ResourceType, int>();
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 10; // Starting amount
            }

            // Initialize buildings
            buildings = new List<Building>();
        }

        public void Start()
        {
            while (true)
            {
                DisplayStatus();
                ProcessTurn();
                CheckWinLose();
                day++;
            }
        }

        private void DisplayStatus()
        {
            Console.WriteLine($"\n--- Day {day} ---");
            Console.WriteLine($"Survivors: {survivors.Count} (Avg Health: {survivors.Average(s => s.Health):F1}, Avg Morale: {survivors.Average(s => s.Morale):F1})");
            Console.WriteLine("Resources:");
            foreach (var res in resources)
            {
                Console.WriteLine($"  {res.Key}: {res.Value}");
            }
            Console.WriteLine("Buildings:");
            if (buildings.Count == 0)
            {
                Console.WriteLine("  None");
            }
            else
            {
                foreach (var building in buildings)
                {
                    Console.WriteLine($"  {building.Name} (Level: {building.Level})");
                }
            }
        }

        private void ProcessTurn()
        {
            Console.WriteLine("\nChoose an action:");
            Console.WriteLine("1. Assign survivors to tasks");
            Console.WriteLine("2. Build a structure");
            Console.WriteLine("3. Explore the map");
            Console.WriteLine("4. Rest all survivors");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            switch (choice)
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
                    RestSurvivors();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Skipping turn.");
                    break;
            }

            // Consume resources daily
            ConsumeDailyResources();

            // Process random event
            HandleRandomEvent();

            // Update survivors and buildings
            UpdateGameState();
        }

        private void AssignTasks()
        {
            Console.WriteLine("Assigning tasks (simplified: all survivors gather resources)");
            // Simple logic: Survivors gather based on their skills (not fully implemented for brevity)
            foreach (var survivor in survivors)
            {
                resources[ResourceType.Food] += random.Next(1, 4);
                resources[ResourceType.Water] += random.Next(1, 4);
                survivor.Health -= random.Next(0, 10); // Risk of injury
            }
        }

        private void BuildStructure()
        {
            Console.WriteLine("Available buildings: Shelter, Farm, Water Purifier");
            Console.Write("Enter building name: ");
            string buildName = Console.ReadLine();
            BuildingType type;
            if (Enum.TryParse(buildName.Replace(" ", ""), true, out type))
            {
                if (resources[ResourceType.BuildingMaterials] >= 5)
                {
                    resources[ResourceType.BuildingMaterials] -= 5;
                    buildings.Add(new Building(type.ToString(), type));
                    Console.WriteLine($"Built {type}!");
                }
                else
                {
                    Console.WriteLine("Not enough building materials.");
                }
            }
            else
            {
                Console.WriteLine("Invalid building.");
            }
        }

        private void Explore()
        {
            Console.WriteLine("Exploring the map (finding random resources).");
            resources[(ResourceType)random.Next(Enum.GetValues(typeof(ResourceType)).Length)] += random.Next(5, 15);
        }

        private void RestSurvivors()
        {
            Console.WriteLine("All survivors rest.");
            foreach (var survivor in survivors)
            {
                survivor.Health = Math.Min(100, survivor.Health + 20);
                survivor.Morale = Math.Min(100, survivor.Morale + 10);
            }
        }

        private void ConsumeDailyResources()
        {
            // Simplified consumption
            int foodNeeded = survivors.Count;
            int waterNeeded = survivors.Count;
            if (resources[ResourceType.Food] >= foodNeeded)
            {
                resources[ResourceType.Food] -= foodNeeded;
            }
            else
            {
                Console.WriteLine("Warning: Not enough food! Survivors are hungry.");
                foreach (var survivor in survivors)
                {
                    survivor.Health -= 10;
                }
            }
            if (resources[ResourceType.Water] >= waterNeeded)
            {
                resources[ResourceType.Water] -= waterNeeded;
            }
            else
            {
                Console.WriteLine("Warning: Not enough water! Survivors are thirsty.");
                foreach (var survivor in survivors)
                {
                    survivor.Health -= 10;
                }
            }
        }

        private void HandleRandomEvent()
        {
            if (random.Next(0, 10) < 3) // 30% chance
            {
                int eventType = random.Next(0, 3);
                switch (eventType)
                {
                    case 0:
                        Console.WriteLine("Event: Alien encounter! Lost some resources.");
                        resources[ResourceType.Food] = Math.Max(0, resources[ResourceType.Food] - 5);
                        break;
                    case 1:
                        Console.WriteLine("Event: Disease outbreak! Survivors' health decreased.");
                        foreach (var survivor in survivors)
                        {
                            survivor.Health -= 15;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Event: Good weather! Found extra resources.");
                        resources[ResourceType.BuildingMaterials] += 10;
                        break;
                }
            }
        }

        private void UpdateGameState()
        {
            // Remove dead survivors
            survivors.RemoveAll(s => s.Health <= 0);
            if (survivors.Count == 0)
            {
                Console.WriteLine("All survivors are dead. Game over!");
                Environment.Exit(0);
            }

            // Update buildings (simple: increase production)
            foreach (var building in buildings)
            {
                if (building.Type == BuildingType.Farm)
                {
                    resources[ResourceType.Food] += 2;
                }
                else if (building.Type == BuildingType.WaterPurifier)
                {
                    resources[ResourceType.Water] += 2;
                }
            }
        }

        private void CheckWinLose()
        {
            // Simple win condition: 10 buildings or day 50
            if (buildings.Count >= 10)
            {
                Console.WriteLine("Congratulations! You have built a thriving colony!");
                Environment.Exit(0);
            }
            if (day > 50)
            {
                Console.WriteLine("Time's up. The colony survives, but barely.");
                Environment.Exit(0);
            }
        }
    }
}