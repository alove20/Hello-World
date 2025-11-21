using System;
using System.Collections.Generic;
using ColonyShipExodus.Models;
using ColonyShipExodus.Util;

namespace ColonyShipExodus.Game
{
    public class GameLoop
    {
        private Map map;
        private Colony colony;
        private EventSystem eventSystem;
        private int turn;

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("=== COLONY SHIP EXODUS ===");
            Console.WriteLine("You are the commander of a crashed colony ship on an alien planet.");
            Console.WriteLine("Manage your survivors, resources, and build a new life!\n");

            InitializeGame();

            bool running = true;
            while (running)
            {
                Console.WriteLine($"\n=== DAY {turn} ===");
                DisplayStatus();

                // Survivor assignment
                AssignSurvivorTasks();

                // Process daily results
                EndOfDaySummary();

                // Event System
                eventSystem.TriggerRandomEvent(colony, map);

                // Ask to continue or quit
                Console.WriteLine("\nPress ENTER to continue to next day, or type Q to quit.");
                var input = Console.ReadLine();
                if (input != null && input.Trim().Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    running = false;
                }
                else
                {
                    turn++;
                    colony.OnTurnEnd();
                }
            }

            Console.WriteLine("Game Over. Thanks for playing Colony Ship Exodus!");
        }

        private void InitializeGame()
        {
            map = new Map(8, 8); // 8x8 grid
            colony = new Colony();
            eventSystem = new EventSystem();

            turn = 1;
        }

        private void DisplayStatus()
        {
            Console.WriteLine("\n--- Colony Resources ---");
            foreach (var kvp in colony.Resources)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\n--- Buildings ---");
            if (colony.Buildings.Count == 0)
            {
                Console.WriteLine("No buildings constructed.");
            }
            else
            {
                foreach (var b in colony.Buildings)
                {
                    Console.WriteLine($"{b.Type} (Status: {b.Status})");
                }
            }

            Console.WriteLine("\n--- Survivors ---");
            foreach (var s in colony.Survivors)
            {
                Console.WriteLine($"{s.Name} | Skill: {s.Skill} | Health: {s.Health} | Morale: {s.Morale} | Assigned: {s.AssignedTask}");
            }
        }

        private void AssignSurvivorTasks()
        {
            Console.WriteLine("\nAssign tasks for your survivors.\nAvailable tasks:");
            Console.WriteLine("1. Scavenge (find resources)");
            Console.WriteLine("2. Build (construct/repair buildings)");
            Console.WriteLine("3. Farm (produce food/water)");
            Console.WriteLine("4. Rest (recover health/morale)");

            foreach (var survivor in colony.Survivors)
            {
                Console.WriteLine($"\nAssign task to {survivor.Name} (Skill: {survivor.Skill} | Health: {survivor.Health} | Morale: {survivor.Morale})");
                Console.Write("Enter task number (1-4): ");
                int taskNum = InputHelper.ReadInt(1, 4);
                survivor.AssignedTask = (TaskType)taskNum;
            }

            // Process tasks
            colony.ProcessSurvivorTasks(map);
        }

        private void EndOfDaySummary()
        {
            Console.WriteLine("\n--- End of Day Summary ---");

            foreach (var survivor in colony.Survivors)
            {
                if (!survivor.IsAlive)
                {
                    Console.WriteLine($"{survivor.Name} has died.");
                }
                else if (survivor.Health < 30)
                {
                    Console.WriteLine($"{survivor.Name} is in poor health.");
                }
                else if (survivor.Morale < 40)
                {
                    Console.WriteLine($"{survivor.Name} is demoralized.");
                }
            }
        }
    }
}