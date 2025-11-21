using System;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            var game = new GameEngine();
            game.Start();
        }
    }
}