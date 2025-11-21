using System;
using System.Collections.Generic;
using ColonyShipExodus.Models;
using ColonyShipExodus.Systems;

namespace ColonyShipExodus.GameCore
{
    public class Game
    {
        private World World { get; set; }
        private List<Survivor> Survivors { get; set; }
        private Inventory Inventory { get; set; }
        private List<Building> Buildings { get; set; }
        private int Day { get; set; }
        private Random Random { get; set; }
        private GameEventSystem EventSystem { get; set; }

        private bool IsGameOver { get; set; } = false;
        private int MaxInitialSurvivors = 6;

        public Game()
        {
            this.Random = new Random();
            this.World = new World(8, 8, Random);
            this.Inventory = new Inventory();
            this.Survivors = SurvivorGenerator.GenerateInitialSurvivors(MaxInitialSurvivors, Random);
            this.Buildings = new List<Building>();
            this.EventSystem = new GameEventSystem();
            this.Day = 1;
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("==== Colony Ship Exodus ====\n");
            Console.WriteLine("You are the commander of a crashed colony ship.");
            Console.WriteLine("Manage your survivors, gather resources, and build a new home on this alien world.\n");
            Console.WriteLine("Press Enter to begin...");
            Console.ReadLine();

            while (!IsGameOver)
            {
                RunDay();
            }

            Console.WriteLine("\nGame Over! Thank you for playing! Press any key to exit.");
            Console.ReadKey();
        }

        private void RunDay()
        {
            Console.Clear();
            Console.WriteLine($"--- DAY {Day} ---\n");

            SurvivorManager.DailyUpdate(Survivors, Buildings, Inventory, Random);
            EventSystem.HandleDailyEvent(Survivors, Inventory, Buildings, World, Random);

            PrintGameState();

            if (Survivors.Count == 0)
            {
                Console.WriteLine("All survivors are dead. The colony has failed.");
                IsGameOver = true;
                return;
            }
            if (Inventory[ResourceType.Food] <= 0 || Inventory[ResourceType.Water] <= 0)
            {
                Console.WriteLine("You have run out of food or water. The colony cannot survive.");
                IsGameOver = true;
                return;
            }

            PlayerTurn();

            Day++;
        }

        private void PrintGameState()
        {
            Console.WriteLine("Survivors:");
            foreach (var s in Survivors)
                Console.WriteLine($"- {s.Name} (Health: {s.Health}, Morale: {s.Morale}, Role: {s.PrimarySkill})");

            Console.WriteLine("\nResources:");
            foreach (ResourceType rt in Enum.GetValues(typeof(ResourceType)))
            {
                Console.WriteLine($"{rt}: {Inventory[rt]}");
            }

            Console.WriteLine("\nBuildings:");
            if (Buildings.Count == 0)
                Console.WriteLine("None");
            else
                foreach (var b in Buildings)
                    Console.WriteLine($"- {b.Name} (Completed: {b.IsCompleted})");

            Console.WriteLine("\nMap Overview:");
            World.PrintMap();
            Console.WriteLine();
        }

        private void PlayerTurn()
        {
            bool turnDone = false;
            while (!turnDone)
            {
                Console.WriteLine("Choose an action:");
                Console.WriteLine("1) Assign survivors to tasks");
                Console.WriteLine("2) Build a new structure");
                Console.WriteLine("3) Explore new land");
                Console.WriteLine("4) End Day");

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
                        ExploreMap();
                        break;
                    case "4":
                        turnDone = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void AssignTasks()
        {
            // Assign each survivor to a task
            List<Survivor> available = new List<Survivor>(Survivors);
            var tasks = new Dictionary<Survivor, string>();

            foreach (var survivor in available)
            {
                Console.WriteLine($"\nAssign a task for {survivor.Name} (Primary: {survivor.PrimarySkill}):");
                Console.WriteLine("1) Scavenge");
                Console.WriteLine("2) Build (if any building is under construction)");
                Console.WriteLine("3) Farm/Gather");
                Console.WriteLine("4) Rest/Recover");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        TaskEngine.DoScavenge(survivor, Inventory, World, Random);
                        break;
                    case "2":
                        if (Buildings.Exists(b => !b.IsCompleted))
                        {
                            TaskEngine.DoBuild(survivor, Buildings, Inventory);
                        }
                        else
                        {
                            Console.WriteLine("No buildings under construction. Assigned to rest.");
                            TaskEngine.DoRest(survivor);
                        }
                        break;
                    case "3":
                        TaskEngine.DoFarmOrGather(survivor, Inventory, World, Random);
                        break;
                    case "4":
                        TaskEngine.DoRest(survivor);
                        break;
                    default:
                        Console.WriteLine("Invalid task. Survivor will rest by default.");
                        TaskEngine.DoRest(survivor);
                        break;
                }
            }
        }

        private void BuildStructure()
        {
            List<BuildingType> buildable = BuildingCatalog.GetAvailableBuildings(Buildings, Inventory);
            if (buildable.Count == 0)
            {
                Console.WriteLine("No available buildings to construct or insufficient resources.");
                return;
            }
            Console.WriteLine("\nChoose a structure to build:");
            for (int i = 0; i < buildable.Count; i++)
            {
                var type = buildable[i];
                var def = BuildingCatalog.GetDefinition(type);
                Console.WriteLine($"{i + 1}) {def.Name}: {def.Description} " +
                    $"(Requires: {def.CostString()})");
            }
            string input = Console.ReadLine();
            if (int.TryParse(input, out int index) && index >= 1 && index <= buildable.Count)
            {
                BuildingType choice = buildable[index - 1];
                var building = BuildingCatalog.CreateBuilding(choice);
                foreach (var (rtype, amt) in building.Cost)
                    Inventory[rtype] -= amt;
                Buildings.Add(building);

                Console.WriteLine($"Started construction of: {building.Name}");
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ExploreMap()
        {
            Console.WriteLine("\nSelect a tile to explore (format: x y), e.g., 2 3");
            string input = Console.ReadLine();
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y) &&
                x >= 0 && y >= 0 && x < World.Width && y < World.Height)
            {
                var tile = World.Map[x, y];
                if (tile.IsExplored)
                {
                    Console.WriteLine("Tile is already explored.");
                }
                else
                {
                    tile.IsExplored = true;
                    Console.WriteLine($"Explored tile at ({x}, {y}): {tile.BiomeType}");
                    if (tile.SpecialDescription != null)
                        Console.WriteLine($"You discovered: {tile.SpecialDescription}");
                }
            }
            else
            {
                Console.WriteLine("Invalid coordinates.");
            }
        }
    }
}