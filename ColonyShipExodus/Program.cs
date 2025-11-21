using System;
using ColonyShipExodus.Game;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Colony Ship Exodus";
            GameLoop game = new GameLoop();
            game.Start();
        }
    }
}