using System;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Colony Ship Exodus";
            
            var game = new Game();
            game.Start();
        }
    }
}