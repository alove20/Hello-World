using System;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Colony Ship Exodus!");
            Console.WriteLine("You are the commander of a crash-landed colony ship on an alien planet.");
            Console.WriteLine("Your goal is to manage survivors, gather resources, and build a self-sufficient colony.");
            Console.WriteLine();

            Game game = new Game();
            game.Start();
        }
    }
}