using System;

namespace BlackjackGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            var game = new GameEngine();
            game.Start();
        }
    }
}