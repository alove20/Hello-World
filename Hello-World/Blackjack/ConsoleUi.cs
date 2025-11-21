using System;
using System.Globalization;
using System.Linq;

namespace Hello_World.Blackjack;

public sealed class ConsoleUi
{
    private readonly ConsoleColor _defaultForeground;

    public ConsoleUi()
    {
        _defaultForeground = Console.ForegroundColor;
    }

    public void Clear()
    {
        Console.Clear();
    }

    public void WriteColored(string text, ConsoleColor color, bool newLine = false)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        if (newLine) Console.WriteLine(text);
        else Console.Write(text);
        Console.ForegroundColor = previous;
    }

    public void WriteBanner()
    {
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
        Console.WriteLine("===============================================");
        Console.WriteLine("              TERMINAL BLACKJACK               ");
        Console.WriteLine("===============================================");
        Console.ResetColor();
    }

    public void ShowBalances(Player player)
    {
        Console.Write("Balance: ");
        WriteColored($"{player.Balance:C}", ConsoleColor.Yellow, true);
        Console.WriteLine();
    }

    public decimal AskForBet(Player player)
    {
        while (true)
        {
            Console.Write("Place your bet (or enter 0 to quit): ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                PrintError("Please enter a bet amount.");
                continue;
            }

            if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
            {
                PrintError("Invalid number. Try again.");
                continue;
            }

            if (amount == 0)
                return 0;

            if (amount < 0)
            {
                PrintError("Bet must be positive.");
                continue;
            }

            if (!player.CanBet(amount))
            {
                PrintError("You don't have enough balance for that bet.");
                continue;
            }

            return amount;
        }
    }

    public string AskAction()
    {
        while (true)
        {
            Console.Write("Choose action: [H]it or [S]tand: ");
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (string.IsNullOrEmpty(input))
            {
                PrintError("Please choose an action.");
                continue;
            }

            if (input is "h" or "hit")
                return "hit";

            if (input is "s" or "stand")
                return "stand";

            PrintError("Invalid choice. Enter H or S.");
        }
    }

    public bool AskPlayAgain()
    {
        while (true)
        {
            Console.Write("Play another hand? (Y/N): ");
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (input is "y" or "yes")
                return true;

            if (input is "n" or "no")
                return false;

            PrintError("Invalid choice. Enter Y or N.");
        }
    }

    public bool AskRestart()
    {
        while (true)
        {
            Console.Write("You are out of money. Restart with initial balance? (Y/N): ");
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (input is "y" or "yes")
                return true;

            if (input is "n" or "no")
                return false;

            PrintError("Invalid choice. Enter Y or N.");
        }
    }

    public void PrintError(string message)
    {
        WriteColored(message + Environment.NewLine, ConsoleColor.Red);
    }

    public void ShowHands(Player player, Dealer dealer, bool hideDealerHoleCard)
    {
        Console.WriteLine();
        WriteColored("Dealer's hand: ", ConsoleColor.Cyan, true);

        if (hideDealerHoleCard && dealer.Hand.Cards.Count > 0)
        {
            var first = dealer.Hand.Cards.First();
            RenderCard(first);
            RenderHiddenCard();
            Console.WriteLine();
        }
        else
        {
            foreach (var c in dealer.Hand.Cards)
                RenderCard(c);
            Console.WriteLine($"  (Total: {dealer.Hand.GetBestValue()})");
        }

        Console.WriteLine();
        WriteColored($"{player.Name}'s hand: ", ConsoleColor.Cyan, true);
        foreach (var c in player.Hand.Cards)
            RenderCard(c);
        Console.WriteLine($"  (Total: {player.Hand.GetBestValue()})");
        Console.WriteLine();
    }

    private void RenderCard(Card card)
    {
        var suitColor = card.GetSuitColor();
        Console.Write("[");
        WriteColored(card.GetRankString(), _defaultForeground);
        WriteColored(card.GetSuitSymbol(), suitColor);
        Console.Write("] ");
    }

    private void RenderHiddenCard()
    {
        WriteColored("[??]", ConsoleColor.DarkGray);
        Console.Write(" ");
    }

    public void AnnounceResult(string message, ConsoleColor color)
    {
        WriteColored(message + Environment.NewLine, color, true);
    }

    public void PauseForUser()
    {
        Console.WriteLine();
        Console.WriteLine("Press ENTER to continue...");
        Console.ReadLine();
    }
}