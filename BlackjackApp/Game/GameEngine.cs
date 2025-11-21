using BlackjackApp.Models;

namespace BlackjackApp.Game;

public class GameEngine
{
    private readonly Deck _deck;
    private readonly Player _player;
    private readonly Dealer _dealer;

    public GameEngine()
    {
        _deck = new Deck();
        _player = new Player(100); // Default starting balance
        _dealer = new Dealer();
    }

    public void Run()
    {
        ConsoleUI.SetupConsole();
        ConsoleUI.PrintWelcome();

        while (true)
        {
            if (_player.Balance <= 0)
            {
                ConsoleUI.PrintMessage("You are out of money! Game Over.", ConsoleColor.Red);
                Console.Write("Restart game with $100? (Y/N): ");
                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                {
                    _player.AdjustBalance(100 - _player.Balance); // Reset to 100
                    Console.WriteLine();
                }
                else
                {
                    break;
                }
            }

            _deck.ReshuffleIfLow();
            PlayRound();

            if (!ConsoleUI.AskToPlayAgain())
            {
                break;
            }
        }

        ConsoleUI.PrintMessage("Thanks for playing! Goodbye.", ConsoleColor.Cyan);
    }

    private void PlayRound()
    {
        _player.ResetHand();
        _dealer.ResetHand();

        // 1. Betting
        decimal bet = ConsoleUI.GetBetAmount(_player.Balance);

        // 2. Initial Deal
        _player.TakeCard(_deck.Draw());
        _dealer.TakeCard(_deck.Draw());
        _player.TakeCard(_deck.Draw());
        _dealer.TakeCard(_deck.Draw());

        bool playerBusted = false;
        bool playerBlackjack = _player.Hand.IsBlackjack;
        
        // Check instant Blackjack
        if (playerBlackjack)
        {
            ConsoleUI.DisplayTable(_player, _dealer, hideDealerCard: false);
            if (_dealer.Hand.IsBlackjack)
            {
                ConsoleUI.PrintMessage("Both have Blackjack! It's a Push.", ConsoleColor.Gray);
                // No money exchanged
            }
            else
            {
                ConsoleUI.PrintMessage("BLACKJACK! You win!", ConsoleColor.Green);
                _player.AdjustBalance(bet * 1.5m); // 3:2 Payout
            }
            return;
        }

        // 3. Player Turn
        while (true)
        {
            ConsoleUI.DisplayTable(_player, _dealer, hideDealerCard: true);

            if (_player.Hand.IsBusted)
            {
                playerBusted = true;
                ConsoleUI.PrintMessage("BUSTED! You went over 21.", ConsoleColor.Red);
                break;
            }

            if (_player.Hand.CalculateScore() == 21)
            {
                // Auto stand on 21
                break;
            }

            string action = ConsoleUI.GetPlayerAction();
            if (action == "HIT")
            {
                _player.TakeCard(_deck.Draw());
                ConsoleUI.PrintMessage("You drew a card...", ConsoleColor.Yellow);
                Thread.Sleep(800); // Small delay for UX
            }
            else
            {
                break;
            }
        }

        // 4. Dealer Turn (only if player didn't bust)
        if (!playerBusted)
        {
            ConsoleUI.DisplayTable(_player, _dealer, hideDealerCard: false);
            ConsoleUI.PrintMessage("Dealer's turn...", ConsoleColor.Magenta);
            Thread.Sleep(1000);

            while (_dealer.ShouldHit())
            {
                _dealer.TakeCard(_deck.Draw());
                ConsoleUI.DisplayTable(_player, _dealer, hideDealerCard: false);
                ConsoleUI.PrintMessage("Dealer Hits.", ConsoleColor.Magenta);
                Thread.Sleep(1000);
            }

            if (_dealer.Hand.IsBusted)
            {
                ConsoleUI.PrintMessage("Dealer BUSTED! You win!", ConsoleColor.Green);
                _player.AdjustBalance(bet);
            }
            else
            {
                // Compare Scores
                int playerScore = _player.Hand.CalculateScore();
                int dealerScore = _dealer.Hand.CalculateScore();

                if (playerScore > dealerScore)
                {
                    ConsoleUI.PrintMessage($"You Win! ({playerScore} vs {dealerScore})", ConsoleColor.Green);
                    _player.AdjustBalance(bet);
                }
                else if (playerScore < dealerScore)
                {
                    ConsoleUI.PrintMessage($"Dealer Wins. ({playerScore} vs {dealerScore})", ConsoleColor.Red);
                    _player.AdjustBalance(-bet);
                }
                else
                {
                    ConsoleUI.PrintMessage($"Push! It's a tie. ({playerScore} vs {dealerScore})", ConsoleColor.Gray);
                    // No balance change
                }
            }
        }
        else
        {
            // Player busted
            _player.AdjustBalance(-bet);
        }
    }
}