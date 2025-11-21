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
            Game game = new Game();
            game.Start();
        }
    }

    public enum ResourceType { Food, Water, Medicine, BuildingMaterials, Energy }
    public enum Skill { Scavenging, Building, Farming, Medical }
    public enum Biome { CrashSite, Forest, Mine, Ruins, Plains }
    public enum EventType { AlienEncounter, DiseaseOutbreak, WeatherStorm, RareResource }

    public class Survivor
    {
        public string Name { get; set; }
        public Skill Skill { get; set; }
        public int Health { get; set; } = 100;
        public int Morale { get; set; } = 50;
        public bool IsAssigned { get; set; } = false;
    }

    public class Resource
    {
        public ResourceType Type { get; set; }
        public int Quantity { get; set; }
    }

    public class MapTile
    {
        public Biome Biome { get; set; }
        public bool Explored { get; set; } = false;
        public List<Resource> Resources { get; set; } = new List<Resource>();
    }

    public class Building
    {
        public string Name { get; set; }
        public bool Built { get; set; } = false;
        public int BuildCost { get; set; } // in BuildingMaterials
        public int EnergyRequirement { get; set; }
    }

    public class Game
    {
        private Random random = new Random();
        private List<Survivor> survivors = new List<Survivor>();
        private Dictionary<ResourceType, int> inventory = new Dictionary<ResourceType, int>();
        private MapTile[,] map = new MapTile[10, 10];
        private List<Building> buildings = new List<Building>();
        private int day = 1;
        private int maxSurvivors = 10;

        public Game()
        {
            InitializeResources();
            GenerateMap();
            GenerateSurvivors();
            InitializeBuildings();
        }

        private void InitializeResources()
        {
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                inventory[type] = 50; // Starting amount
            }
        }

        private void GenerateMap()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Biome biome = (Biome)random.Next(Enum.GetNames(typeof(Biome)).Length);
                    map[x, y] = new MapTile { Biome = biome };
                    // Add some random resources
                    if (random.Next(3) == 0) // 33% chance
                    {
                        ResourceType resType = (ResourceType)random.Next(Enum.GetNames(typeof(ResourceType)).Length);
                        map[x, y].Resources.Add(new Resource { Type = resType, Quantity = random.Next(10, 50) });
                    }
                }
            }
            // Crash site at center
            map[5, 5].Biome = Biome.CrashSite;
            map[5, 5].Explored = true;
        }

        private void GenerateSurvivors()
        {
            for (int i = 0; i < maxSurvivors; i++)
            {
                survivors.Add(new Survivor
                {
                    Name = $"Survivor {i + 1}",
                    Skill = (Skill)random.Next(Enum.GetNames(typeof(Skill)).Length)
                });
            }
        }

        private void InitializeBuildings()
        {
            buildings.Add(new Building { Name = "Shelter", BuildCost = 20, EnergyRequirement = 5 });
            buildings.Add(new Building { Name = "Farm", BuildCost = 30, EnergyRequirement = 10 });
            buildings.Add(new Building { Name = "Water Purifier", BuildCost = 25, EnergyRequirement = 8 });
            buildings.Add(new Building { Name = "Infirmary", BuildCost = 35, EnergyRequirement = 12 });
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine($"\n--- Day {day} ---");
                DisplayStatus();
                ProcessDay();
                day++;
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }

        private void DisplayStatus()
        {
            Console.WriteLine("Resources:");
            foreach (var res in inventory)
            {
                Console.WriteLine($"{res.Key}: {res.Value}");
            }
            Console.WriteLine("Survivors:");
            foreach (var survivor in survivors)
            {
                Console.WriteLine($"{survivor.Name} - Skill: {survivor.Skill}, Health: {survivor.Health}, Morale: {survivor.Morale}");
            }
            Console.WriteLine("Buildings:");
            foreach (var building in buildings)
            {
                Console.WriteLine($"{building.Name}: {(building.Built ? "Built" : "Not Built")}");
            }
        }

        private void ProcessDay()
        {
            // Assign survivors
            AssignSurvivors();

            // Gather resources
            GatherResources();

            // Consume resources
            ConsumeResources();

            // Handle events
            HandleEvent();

            // Build if possible
            Build();

            // Update survivors
            UpdateSurvivors();
        }

        private void AssignSurvivors()
        {
            // Simple auto-assignment for demo
            int assigned = 0;
            foreach (var survivor in survivors.Where(s => !s.IsAssigned))
            {
                if (assigned < 5) // Max assignments
                {
                    survivor.IsAssigned = true;
                    assigned++;
                }
            }
        }

        private void GatherResources()
        {
            foreach (var survivor in survivors.Where(s => s.IsAssigned))
            {
                switch (survivor.Skill)
                {
                    case Skill.Scavenging:
                        inventory[ResourceType.BuildingMaterials] += random.Next(5, 15);
                        break;
                    case Skill.Farming:
                        inventory[ResourceType.Food] += random.Next(10, 20);
                        break;
                    case Skill.Medical:
                        // Could help with medicine production
                        inventory[ResourceType.Medicine] += random.Next(3, 10);
                        break;
                    case Skill.Building:
                        // Assists in building
                        break;
                }
            }
        }

        private void ConsumeResources()
        {
            int consumption = survivors.Count;
            inventory[ResourceType.Food] -= consumption;
            inventory[ResourceType.Water] -= consumption;
            if (inventory[ResourceType.Food] < 0) inventory[ResourceType.Food] = 0;
            if (inventory[ResourceType.Water] < 0) inventory[ResourceType.Water] = 0;
        }

        private void HandleEvent()
        {
            if (random.Next(5) == 0) // 20% chance
            {
                EventType eventType = (EventType)random.Next(Enum.GetNames(typeof(EventType)).Length);
                Console.WriteLine($"Event: {eventType}");
                switch (eventType)
                {
                    case EventType.AlienEncounter:
                        foreach (var survivor in survivors)
                        {
                            survivor.Health -= 10;
                        }
                        break;
                    case EventType.DiseaseOutbreak:
                        inventory[ResourceType.Medicine] -= 20;
                        break;
                    case EventType.WeatherStorm:
                        inventory[ResourceType.Energy] -= 10;
                        break;
                    case EventType.RareResource:
                        inventory[ResourceType.BuildingMaterials] += 50;
                        break;
                }
            }
        }

        private void Build()
        {
            foreach (var building in buildings.Where(b => !b.Built))
            {
                if (inventory[ResourceType.BuildingMaterials] >= building.BuildCost)
                {
                    building.Built = true;
                    inventory[ResourceType.BuildingMaterials] -= building.BuildCost;
                    Console.WriteLine($"{building.Name} built!");
                    // Add benefits, e.g., increase production
                }
            }
        }

        private void UpdateSurvivors()
        {
            foreach (var survivor in survivors)
            {
                if (survivor.Health <= 0)
                {
                    survivors.Remove(survivor);
                    continue;
                }
                survivor.Morale += random.Next(-5, 10);
                if (survivor.Morale < 0) survivor.Morale = 0;
                if (survivor.Morale > 100) survivor.Morale = 100;
                survivor.IsAssigned = false; // Reset for next day
            }
        }
    }
}