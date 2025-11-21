using Blackjack.Models;

namespace Blackjack.UI;

public static class ConsoleInterface
{
    public static void PrintTitle()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==========================================");
        Console.WriteLine("           TERMINAL BLACKJACK             ");
        Console.WriteLine("==========================================");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void PrintStatus(decimal balance, decimal currentBet)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Balance: ${balance:F2}");
        if (currentBet > 0)
        {
            Console.Write($" | Current Bet: ${currentBet:F2}");
        }
        Console.WriteLine();
        Console.ResetColor();
        Console.WriteLine(new string('-', 40));
    }

    public static void PrintHand(string owner, Hand hand, bool hideSecondCard = false)
    {
        Console.ForegroundColor = owner == "Dealer" ? ConsoleColor.Red : ConsoleColor.Green;
        Console.Write($"{owner}'s Hand: ");
        Console.ResetColor();

        if (hand.Cards.Count == 0)
        {
            Console.WriteLine("[Empty]");
            return;
        }

        if (hideSecondCard && hand.Cards.Count > 1)
        {
            // Show first card, hide second
            PrintCard(hand.Cards[0]);
            Console.Write(" [??]");
            Console.WriteLine();
        }
        else
        {
            foreach (var card in hand.Cards)
            {
                PrintCard(card);
                Console.Write(" ");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($" ({hand.CalculateValue()})");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private static void PrintCard(Card card)
    {
        // Color suits
        ConsoleColor color = (card.Suit == Suit.Hearts || card.Suit == Suit.Diamonds) 
            ? ConsoleColor.Red 
            : ConsoleColor.White;

        Console.ForegroundColor = ConsoleColor.DarkGray; // Bracket color
        Console.Write("[");
        Console.ForegroundColor = color;
        Console.Write(card.ToString());
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("]");
        Console.ResetColor();
    }

    public static void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"\n>> {message}");
        Console.ResetColor();
    }

    public static decimal GetBetAmount(decimal balance)
    {
        while (true)
        {
            Console.Write($"\nEnter bet amount (Max ${balance:F0}): ");
            string? input = Console.ReadLine();
            
            if (decimal.TryParse(input, out decimal bet))
            {
                if (bet > 0 && bet <= balance)
                {
                    return bet;
                }
                PrintMessage("Invalid amount. You cannot bet more than you have or <= 0.", ConsoleColor.DarkRed);
            }
            else
            {
                PrintMessage("Please enter a valid number.", ConsoleColor.DarkRed);
            }
        }
    }

    public static string GetPlayerAction()
    {
        while (true)
        {
            Console.Write("\nAction: (H)it or (S)tand? ");
            string? input = Console.ReadLine()?.Trim().ToUpper();
            if (input == "H" || input == "HIT") return "H";
            if (input == "S" || input == "STAND") return "S";
            
            PrintMessage("Invalid input. Press 'H' or 'S'.", ConsoleColor.DarkYellow);
        }
    }
}