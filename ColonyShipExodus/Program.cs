using ColonyShipExodus.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColonyShipExodus
{
    class Program
    {
        static List<Survivor> survivors = new List<Survivor>();
        static Dictionary<ResourceType, Resource> resources = new Dictionary<ResourceType, Resource>();
        static List<Building> buildings = new List<Building>();
        static MapTile[,] map;
        static int mapSize = 10;
        static Random random = new Random();
        static int currentDay = 1;

        static void Main(string[] args)
        {
            Console.WriteLine("Colony Ship Exodus");
            Console.WriteLine("You have crash-landed on an alien planet. Your goal: survive and build a new colony.");

            InitializeGame();
            GameLoop();
        }

        static void InitializeGame()
        {
            // Initialize Survivors
            survivors.Add(new Survivor("Alice"));
            survivors.Add(new Survivor("Bob"));
            survivors.Add(new Survivor("Charlie"));

            // Initialize Resources
            resources.Add(ResourceType.Food, new Resource(ResourceType.Food, 100));
            resources.Add(ResourceType.Water, new Resource(ResourceType.Water, 50));
            resources.Add(ResourceType.Medicine, new Resource(ResourceType.Medicine, 20));
            resources.Add(ResourceType.BuildingMaterials, new Resource(ResourceType.BuildingMaterials, 150));
            resources.Add(ResourceType.Energy, new Resource(ResourceType.Energy, 75));

            // Initialize Buildings
            buildings.Add(new Building(BuildingType.Shelter));
            buildings.Add(new Building(BuildingType.Farm));
            buildings.Add(new Building(BuildingType.WaterPurifier));

            // Initialize Map
            map = new MapTile[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    BiomeType biome = (BiomeType)random.Next(Enum.GetNames(typeof(BiomeType)).Length);
                    map[i, j] = new MapTile(biome);
                }
            }
        }

        static void GameLoop()
        {
            while (true)
            {
                Console.WriteLine($"\n--- Day {currentDay} ---");
                DisplayStatus();
                GetPlayerInput();
                UpdateGameState();
                currentDay++;
            }
        }

        static void DisplayStatus()
        {
            Console.WriteLine("\n--- Colony Status ---");
            Console.WriteLine("Survivors: " + survivors.Count);
            foreach (var resource in resources)
            {
                Console.WriteLine($"{resource.Key}: {resource.Value.Quantity}");
            }
            Console.WriteLine("\n--- Building Status ---");
            foreach (var building in buildings)
            {
                Console.WriteLine($"{building.Type}: {(building.IsBuilt ? "Built" : "Building (" + building.BuildProgress + "%)")}");
            }
        }

        static void GetPlayerInput()
        {
            Console.WriteLine("\nWhat do you want to do? (Explore, Gather, Build, Assign, Rest)");
            string input = Console.ReadLine().ToLower();

            switch (input)
            {
                case "explore":
                    Explore();
                    break;
                case "gather":
                    GatherResources();
                    break;
                case "build":
                    BuildStructure();
                    break;
                case "assign":
                    AssignSurvivor();
                    break;
                case "rest":
                    RestSurvivor();
                    break;
                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
        }

        static void Explore()
        {
            Console.WriteLine("Enter coordinates to explore (x,y):");
            string coords = Console.ReadLine();
            string[] parts = coords.Split(',');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            {
                Console.WriteLine("Invalid coordinates.");
                return;
            }

            if (x < 0 || x >= mapSize || y < 0 || y >= mapSize)
            {
                Console.WriteLine("Coordinates out of bounds.");
                return;
            }

            MapTile tile = map[x, y];
            tile.IsExplored = true;
            Console.WriteLine($"Explored tile at ({x},{y}). Biome: {tile.Biome}, Resources: {tile.Resources}");

            // Small chance to find extra resources
            if (random.Next(10) < 3)
            {
                int foundResources = random.Next(10, 30);
                Console.WriteLine($"You found extra resources! Added {foundResources} Building Materials.");
                resources[ResourceType.BuildingMaterials].Quantity += foundResources;
            }
        }

        static void GatherResources()
        {
            Console.WriteLine("Which resource to gather? (Food, Water, BuildingMaterials, Energy)");
            string resourceInput = Console.ReadLine().ToLower();
            ResourceType resourceType;

            if (!Enum.TryParse(resourceInput, true, out resourceType))
            {
                Console.WriteLine("Invalid resource type.");
                return;
            }

            Console.WriteLine("How many survivors to assign?");
            if (!int.TryParse(Console.ReadLine(), out int numSurvivors))
            {
                Console.WriteLine("Invalid number of survivors.");
                return;
            }

            if (numSurvivors > survivors.Count(s => !s.IsAssigned))
            {
                Console.WriteLine("Not enough unassigned survivors.");
                return;
            }

            // Assign survivors (basic assignment - could be improved)
            int assignedCount = 0;
            foreach (var survivor in survivors.Where(s => !s.IsAssigned).Take(numSurvivors))
            {
                survivor.IsAssigned = true;
                assignedCount++;
            }

            int gatheredAmount = numSurvivors * 5; // Example: Each survivor gathers 5 units
            resources[resourceType].Quantity += gatheredAmount;

            Console.WriteLine($"Gathered {gatheredAmount} {resourceType}.");

            // Unassign survivors after gathering
            foreach (var survivor in survivors.Where(s => s.IsAssigned).Take(assignedCount))
            {
                survivor.IsAssigned = false;
            }
        }

        static void BuildStructure()
        {
            Console.WriteLine("Which building to work on? (Shelter, Farm, WaterPurifier)");
            string buildingInput = Console.ReadLine().ToLower();
            BuildingType buildingType;

            if (!Enum.TryParse(buildingInput, true, out buildingType))
            {
                Console.WriteLine("Invalid building type.");
                return;
            }

            Building building = buildings.FirstOrDefault(b => b.Type == buildingType);
            if (building == null)
            {
                Console.WriteLine("Building not found.");
                return;
            }

            if (building.IsBuilt)
            {
                Console.WriteLine("Building is already built.");
                return;
            }

            Console.WriteLine("How many survivors to assign?");
            if (!int.TryParse(Console.ReadLine(), out int numSurvivors))
            {
                Console.WriteLine("Invalid number of survivors.");
                return;
            }

            if (numSurvivors > survivors.Count(s => !s.IsAssigned))
            {
                Console.WriteLine("Not enough unassigned survivors.");
                return;
            }

            // Assign survivors (basic assignment - could be improved)
            int assignedCount = 0;
            foreach (var survivor in survivors.Where(s => !s.IsAssigned).Take(numSurvivors))
            {
                survivor.IsAssigned = true;
                assignedCount++;
            }

            int buildProgress = numSurvivors * 10; // Example: Each survivor adds 10% progress
            building.BuildProgress += buildProgress;

            Console.WriteLine($"Building progress: {building.BuildProgress}%");

            if (building.BuildProgress >= 100)
            {
                building.IsBuilt = true;
                Console.WriteLine($"{building.Type} is now built!");
            }

            // Unassign survivors after building
            foreach (var survivor in survivors.Where(s => s.IsAssigned).Take(assignedCount))
            {
                survivor.IsAssigned = false;
            }
        }

        static void AssignSurvivor()
        {
            Console.WriteLine("Not implemented yet.");
        }

        static void RestSurvivor()
        {
            Console.WriteLine("Not implemented yet.");
        }

        static void UpdateGameState()
        {
            // Consume resources
            resources[ResourceType.Food].Quantity -= survivors.Count * 2;
            resources[ResourceType.Water].Quantity -= survivors.Count;

            if (resources[ResourceType.Food].Quantity < 0)
            {
                Console.WriteLine("Starvation! Some survivors may have died.");
                // Implement survivor death logic here
                resources[ResourceType.Food].Quantity = 0;
            }

            if (resources[ResourceType.Water].Quantity < 0)
            {
                Console.WriteLine("Dehydration! Some survivors may have suffered.");
                // Implement survivor health impact logic here
                resources[ResourceType.Water].Quantity = 0;
            }

            // Random events
            if (random.Next(10) < 2)
            {
                HandleRandomEvent();
            }
        }

        static void HandleRandomEvent()
        {
            Console.WriteLine("\n--- Random Event! ---");
            int eventType = random.Next(3); // 0, 1, 2

            switch (eventType)
            {
                case 0:
                    Console.WriteLine("A meteor shower has struck the area! -10 Building Materials.");
                    resources[ResourceType.BuildingMaterials].Quantity -= 10;
                    if (resources[ResourceType.BuildingMaterials].Quantity < 0)
                    {
                        resources[ResourceType.BuildingMaterials].Quantity = 0;
                    }
                    break;
                case 1:
                    Console.WriteLine("You found a cache of medicine! +15 Medicine.");
                    resources[ResourceType.Medicine].Quantity += 15;
                    break;
                case 2:
                    Console.WriteLine("A wild animal attacked the colony! -5 Health to a random survivor.");
                    if (survivors.Count > 0)
                    {
                        Survivor randomSurvivor = survivors[random.Next(survivors.Count)];
                        randomSurvivor.Health -= 5;
                        Console.WriteLine($"{randomSurvivor.Name} was injured!");
                        if (randomSurvivor.Health <= 0)
                        {
                            Console.WriteLine($"{randomSurvivor.Name} has died!");
                            survivors.Remove(randomSurvivor);
                        }

                    }
                    break;
            }
        }
    }
}