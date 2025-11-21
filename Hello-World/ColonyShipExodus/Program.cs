using System;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Colony Ship Exodus";
            Console.ForegroundColor = ConsoleColor.Green;
            
            ShowIntro();
            
            Game game = new Game();
            game.Start();
            
            Console.ResetColor();
        }

        static void ShowIntro()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              COLONY SHIP EXODUS                                ║");
            Console.WriteLine("║              ══════════════════                                ║");
            Console.WriteLine("║                                                                ║");
            Console.WriteLine("║  Your colony ship has crash-landed on an alien planet.         ║");
            Console.WriteLine("║  As commander, you must manage survivors, explore the          ║");
            Console.WriteLine("║  surrounding area, gather resources, and build a new colony.   ║");
            Console.WriteLine("║                                                                ║");
            Console.WriteLine("║  Your decisions will determine the fate of your people.        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("Press any key to begin...");
            Console.ReadKey();
        }
    }
}