using System;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===========================================");
            Console.WriteLine("    COLONY SHIP EXODUS - SURVIVAL GAME    ");
            Console.WriteLine("===========================================");
            Console.WriteLine();
            
            Game game = new Game();
            game.Initialize();
            game.Run();
        }
    }
}