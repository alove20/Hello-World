using BlackjackApp.Models;

namespace BlackjackApp.Game;

public static class ConsoleHelper
{
    public static void PrintHeader()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("       ♠ ♥ ♦ ♣ BLACKJACK ♣ ♦ ♥ ♠        ");
        Console.WriteLine("========================================");
        Console.ResetColor();
    }

    public static void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void PrintCard(Card card)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = card.GetColor();
        Console.Write($" {card} ");
        Console.ResetColor();
        Console.Write(" ");
    }

    public static void PrintHiddenCard()
    {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" ?? ");
        Console.ResetColor();
        Console.Write(" ");
    }
}