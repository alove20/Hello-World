using System;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            GameEngine game = new GameEngine(playerName, 100);
            game.StartGame();
        }
    }
}