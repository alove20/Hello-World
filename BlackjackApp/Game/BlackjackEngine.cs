using BlackjackApp.Models;

namespace BlackjackApp.Game;

public class BlackjackEngine
{
    private readonly Deck _deck;
    private readonly Player _player;
    private readonly Dealer _dealer;
    private const decimal StartingBalance = 100m;

    public BlackjackEngine()
    {
        _deck = new Deck();
        _player = new Player("Player", StartingBalance);
        _dealer = new Dealer();
    }

    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8; // For Card Suits

        while (true)
        {
            Console.Clear();
            ConsoleHelper.PrintHeader();
            
            if (_player.Balance <= 0)
            {
                ConsoleHelper.PrintMessage("\nYou are out of money! Game Over.", ConsoleColor.Red);
                Console.WriteLine("Press 'R' to restart or any other key to quit.");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    _player.Balance = StartingBalance;
                    continue;
                }
                break;
            }

            PlayRound();

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }
    }

    private void PlayRound()
    {
        // 1. Setup Round
        _dealer.Hand.Clear();
        _player.Hand.Clear();
        _deck.Initialize(); // Reshuffle every game for simplicity/randomness

        // 2. Betting Phase
        ConsoleHelper.PrintMessage($"\nCurrent Balance: ${_player.Balance}", ConsoleColor.Yellow);
        decimal betAmount = GetBetAmount();

        // 3. Initial Deal
        _player.Hand.AddCard(_deck.Draw());
        _dealer.Hand.AddCard(_deck.Draw());
        _player.Hand.AddCard(_deck.Draw());
        _dealer.Hand.AddCard(_deck.Draw());

        // Check for Player Blackjack immediately
        if (_player.Hand.IsBlackjack())
        {
            DisplayTable(hideDealerCard: false);
            if (_dealer.Hand.IsBlackjack())
            {
                ConsoleHelper.PrintMessage("\nBoth have Blackjack! Push.", ConsoleColor.Yellow);
                _player.Balance += betAmount; // Return bet
            }
            else
            {
                ConsoleHelper.PrintMessage("\nBLACKJACK! You win 3:2 payout!", ConsoleColor.Green);
                _player.Balance += betAmount * 2.5m; // Bet + 1.5x winnings
            }
            return;
        }

        // 4. Player Turn
        bool playerBusted = false;
        while (true)
        {
            DisplayTable(hideDealerCard: true);

            // Check valid moves
            if (_player.Hand.CalculateScore() == 21)
            {
                Console.WriteLine("\nYou have 21! Auto-standing.");
                break;
            }

            Console.WriteLine("\nAction: [H]it or [S]tand?");
            char action = char.ToUpper(Console.ReadKey(true).KeyChar);

            if (action == 'H')
            {
                Card newCard = _deck.Draw();
                _player.Hand.AddCard(newCard);
                Console.WriteLine($"You drew: {newCard}");
                Thread.Sleep(600); // Small delay for UX

                if (_player.Hand.IsBusted())
                {
                    DisplayTable(hideDealerCard: false);
                    ConsoleHelper.PrintMessage("\nBUST! You went over 21.", ConsoleColor.Red);
                    playerBusted = true;
                    break;
                }
            }
            else if (action == 'S')
            {
                Console.WriteLine(" You stand.");
                break;
            }
            else
            {
                Console.WriteLine(" Invalid input.");
            }
        }

        if (playerBusted)
        {
            // Player lost bet (already deducted), nothing to add
            return; 
        }

        // 5. Dealer Turn
        Console.WriteLine("\nDealer's turn...");
        DisplayTable(hideDealerCard: false);
        Thread.Sleep(1000);

        while (_dealer.Hand.CalculateScore() < 17)
        {
            Card newCard = _deck.Draw();
            _dealer.Hand.AddCard(newCard);
            Console.WriteLine($"Dealer draws: {newCard}");
            DisplayTable(hideDealerCard: false);
            Thread.Sleep(1000);
        }

        // 6. Resolution
        int playerScore = _player.Hand.CalculateScore();
        int dealerScore = _dealer.Hand.CalculateScore();

        Console.WriteLine($"\nFinal: Player [{playerScore}] vs Dealer [{dealerScore}]");

        if (_dealer.Hand.IsBusted())
        {
            ConsoleHelper.PrintMessage("Dealer Busts! You Win!", ConsoleColor.Green);
            _player.Balance += betAmount * 2;
        }
        else if (playerScore > dealerScore)
        {
            ConsoleHelper.PrintMessage("You Win!", ConsoleColor.Green);
            _player.Balance += betAmount * 2;
        }
        else if (playerScore < dealerScore)
        {
            ConsoleHelper.PrintMessage("Dealer Wins.", ConsoleColor.Red);
            // Bet lost
        }
        else
        {
            ConsoleHelper.PrintMessage("Push (Tie). Bet returned.", ConsoleColor.Yellow);
            _player.Balance += betAmount;
        }
    }

    private decimal GetBetAmount()
    {
        while (true)
        {
            Console.Write("Place your bet: ");
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal bet) && bet > 0)
            {
                if (bet > _player.Balance)
                {
                    ConsoleHelper.PrintMessage("Insufficient funds.", ConsoleColor.Red);
                }
                else
                {
                    _player.Balance -= bet;
                    return bet;
                }
            }
            else
            {
                ConsoleHelper.PrintMessage("Invalid amount.", ConsoleColor.Red);
            }
        }
    }

    private void DisplayTable(bool hideDealerCard)
    {
        Console.Clear();
        ConsoleHelper.PrintHeader();
        Console.WriteLine();

        // Dealer Section
        Console.WriteLine("Dealer's Hand:");
        if (hideDealerCard)
        {
            // Show first card, hide second
            if (_dealer.Hand.Cards.Count > 0)
            {
                ConsoleHelper.PrintCard(_dealer.Hand.Cards[0]);
                ConsoleHelper.PrintHiddenCard();
            }
            Console.WriteLine("\nVal: ?");
        }
        else
        {
            foreach (var card in _dealer.Hand.Cards)
            {
                ConsoleHelper.PrintCard(card);
            }
            Console.WriteLine($"\nVal: {_dealer.Hand.CalculateScore()}");
        }

        Console.WriteLine("\n----------------------------------------");

        // Player Section
        Console.WriteLine($"Player's Hand (${_player.Balance}):");
        foreach (var card in _player.Hand.Cards)
        {
            ConsoleHelper.PrintCard(card);
        }
        Console.WriteLine($"\nVal: {_player.Hand.CalculateScore()}");
        Console.WriteLine("----------------------------------------");
    }
}