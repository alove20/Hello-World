using BlackjackApp.Models;
using System.Text;

namespace BlackjackApp.Game;

public static class ConsoleUI
{
    public static void SetupConsole()
    {
        Console.Title = "Blackjack .NET 9";
        Console.OutputEncoding = Encoding.UTF8;
    }

    public static void PrintWelcome()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("      Welcome to Terminal Blackjack     ");
        Console.WriteLine("========================================");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static decimal GetBetAmount(decimal balance)
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Current Balance: ${balance}. Enter bet amount: ");
            Console.ResetColor();
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal bet))
            {
                if (bet > 0 && bet <= balance)
                {
                    return bet;
                }
                if (bet > balance)
                    PrintMessage("Insufficient funds.", ConsoleColor.Red);
                else
                    PrintMessage("Bet must be greater than 0.", ConsoleColor.Red);
            }
            else
            {
                PrintMessage("Please enter a valid number.", ConsoleColor.Red);
            }
        }
    }

    public static string GetPlayerAction()
    {
        while (true)
        {
            Console.Write("Action [(H)it / (S)tand]: ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            if (input == "H" || input == "HIT") return "HIT";
            if (input == "S" || input == "STAND") return "STAND";
            PrintMessage("Invalid action. Please type 'H' or 'S'.", ConsoleColor.Red);
        }
    }

    public static void DisplayTable(Player player, Dealer dealer, bool hideDealerCard)
    {
        Console.Clear();
        
        // Header
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("=============== BLACKJACK TABLE ===============");
        Console.ResetColor();

        // Dealer Hand
        Console.WriteLine("\nDealer's Hand:");
        if (hideDealerCard && dealer.Hand.Cards.Count > 0)
        {
            PrintCard(dealer.Hand.Cards[0]);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" [Hidden Card] ");
            Console.ResetColor();
            Console.WriteLine("\nScore: ?");
        }
        else
        {
            foreach (var card in dealer.Hand.Cards)
            {
                PrintCard(card);
            }
            Console.WriteLine($"\nScore: {dealer.Hand.CalculateScore()}");
        }

        Console.WriteLine("\n-----------------------------------------------");

        // Player Hand
        Console.WriteLine($"\nPlayer's Hand (Balance: ${player.Balance}):");
        foreach (var card in player.Hand.Cards)
        {
            PrintCard(card);
        }
        Console.WriteLine($"\nScore: {player.Hand.CalculateScore()}");
        
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n===============================================\n");
        Console.ResetColor();
    }

    private static void PrintCard(Card card)
    {
        var suitSymbol = card.Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };

        var color = (card.Suit == Suit.Hearts || card.Suit == Suit.Diamonds) 
            ? ConsoleColor.Red 
            : ConsoleColor.Cyan; // Cyan is often easier to read than dark blue on black

        Console.ForegroundColor = color;
        Console.Write($" [{card.Rank} {suitSymbol}] ");
        Console.ResetColor();
    }

    public static void PrintMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static bool AskToPlayAgain()
    {
        Console.WriteLine();
        Console.Write("Play another round? (Y/N): ");
        var key = Console.ReadKey(intercept: true);
        Console.WriteLine();
        return key.Key == ConsoleKey.Y;
    }
}