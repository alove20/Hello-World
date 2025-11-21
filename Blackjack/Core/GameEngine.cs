using Blackjack.Models;
using Blackjack.UI;

namespace Blackjack.Core;

public class GameEngine
{
    private readonly Deck _deck;
    private readonly Hand _playerHand;
    private readonly Hand _dealerHand;
    private decimal _balance;
    
    // Configuration
    private const decimal InitialBalance = 100.00m;
    private const decimal BlackjackPayoutMultiplier = 1.5m; // 3:2

    public GameEngine()
    {
        _deck = new Deck();
        _playerHand = new Hand();
        _dealerHand = new Hand();
        _balance = InitialBalance;
    }

    public void Run()
    {
        while (_balance > 0)
        {
            PlayRound();
            
            if (_balance <= 0)
            {
                ConsoleInterface.PrintMessage("You are out of money! Game Over.", ConsoleColor.DarkRed);
                Console.WriteLine("Press 'R' to restart with $100, or any other key to quit.");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.R)
                {
                    _balance = InitialBalance;
                    continue;
                }
                else
                {
                    break;
                }
            }
            else
            {
                Console.WriteLine("\nPress any key to play the next hand (or 'Q' to quit)...");
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Q) break;
            }
        }

        ConsoleInterface.PrintMessage("Thanks for playing!", ConsoleColor.Cyan);
    }

    private void PlayRound()
    {
        // Cleanup from previous round
        _playerHand.Clear();
        _dealerHand.Clear();
        // We can shuffle every round to prevent card counting strategies in this simple version
        // or check remaining count. For simplicity and fairness in fake money:
        if (new Random().Next(0, 5) == 0) // Random shuffle occasionally
        {
            _deck.Reset(); 
            // Note: Deck.Reset shuffles.
        }

        ConsoleInterface.PrintTitle();
        ConsoleInterface.PrintStatus(_balance, 0);

        decimal bet = ConsoleInterface.GetBetAmount(_balance);
        _balance -= bet;

        // Deal Initial Cards
        _playerHand.AddCard(_deck.Draw());
        _dealerHand.AddCard(_deck.Draw());
        _playerHand.AddCard(_deck.Draw());
        _dealerHand.AddCard(_deck.Draw());

        RefreshTable(bet, hideDealerCard: true);

        // Check Initial Blackjack
        if (_playerHand.IsBlackjack())
        {
            // Check if dealer also has Blackjack
            if (_dealerHand.IsBlackjack())
            {
                RefreshTable(bet, hideDealerCard: false);
                ConsoleInterface.PrintMessage("Push! Both have Blackjack.", ConsoleColor.Yellow);
                _balance += bet; // Return bet
            }
            else
            {
                RefreshTable(bet, hideDealerCard: false);
                decimal winAmount = bet + (bet * BlackjackPayoutMultiplier);
                ConsoleInterface.PrintMessage($"Blackjack! You win ${winAmount:F2}!", ConsoleColor.Green);
                _balance += winAmount;
            }
            return;
        }

        // Player Turn
        bool playerBusted = false;
        while (true)
        {
            string action = ConsoleInterface.GetPlayerAction();
            if (action == "H")
            {
                Card c = _deck.Draw();
                _playerHand.AddCard(c);
                ConsoleInterface.PrintMessage($"Dealt: {c}", ConsoleColor.White);
                
                RefreshTable(bet, hideDealerCard: true);

                if (_playerHand.IsBusted())
                {
                    playerBusted = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (playerBusted)
        {
            ConsoleInterface.PrintMessage("Bust! You went over 21. Dealer wins.", ConsoleColor.Red);
            return; // Bet already deducted
        }

        // Dealer Turn
        ConsoleInterface.PrintMessage("Dealer's turn...", ConsoleColor.Magenta);
        RefreshTable(bet, hideDealerCard: false);
        Thread.Sleep(1000); // Add suspense

        while (_dealerHand.CalculateValue() < 17)
        {
            Card c = _deck.Draw();
            _dealerHand.AddCard(c);
            Console.WriteLine($"Dealer hits... gets {c}");
            Thread.Sleep(1000);
            RefreshTable(bet, hideDealerCard: false);
        }

        // Determine Winner
        int playerVal = _playerHand.CalculateValue();
        int dealerVal = _dealerHand.CalculateValue();

        if (_dealerHand.IsBusted())
        {
            ConsoleInterface.PrintMessage($"Dealer busts ({dealerVal})! You win!", ConsoleColor.Green);
            _balance += bet * 2;
        }
        else if (playerVal > dealerVal)
        {
            ConsoleInterface.PrintMessage($"You win! {playerVal} beats {dealerVal}.", ConsoleColor.Green);
            _balance += bet * 2;
        }
        else if (dealerVal > playerVal)
        {
            ConsoleInterface.PrintMessage($"Dealer wins! {dealerVal} beats {playerVal}.", ConsoleColor.Red);
        }
        else
        {
            ConsoleInterface.PrintMessage($"Push! Tie at {playerVal}.", ConsoleColor.Yellow);
            _balance += bet;
        }
    }

    private void RefreshTable(decimal currentBet, bool hideDealerCard)
    {
        ConsoleInterface.PrintTitle();
        ConsoleInterface.PrintStatus(_balance, currentBet);
        Console.WriteLine();
        ConsoleInterface.PrintHand("Dealer", _dealerHand, hideDealerCard);
        Console.WriteLine();
        ConsoleInterface.PrintHand("Player", _playerHand, false);
    }
}