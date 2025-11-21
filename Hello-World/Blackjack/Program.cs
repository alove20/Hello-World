using System;

namespace Hello_World.Blackjack;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var game = new GameEngine();
        game.Run();
    }
}