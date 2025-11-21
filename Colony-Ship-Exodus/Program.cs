using System;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== COLONY SHIP EXODUS ===");
            Console.WriteLine("Your ship has crash-landed on an alien world.");
            Console.WriteLine("Lead your survivors to build a thriving colony!");
            Console.WriteLine();

            var game = new GameEngine();
            game.StartGame();
        }
    }
}