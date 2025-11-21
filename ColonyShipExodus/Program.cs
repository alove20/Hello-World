using System;
using System.Collections.Generic;
using ColonyShipExodus.GameCore;

namespace ColonyShipExodus
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Game game = new Game();
            game.Start();
        }
    }
}