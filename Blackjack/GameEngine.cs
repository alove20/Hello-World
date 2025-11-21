using System;

namespace Blackjack
{
    public class GameEngine
    {
        private readonly Player player;
        private readonly Dealer dealer;
        private readonly Deck deck;
        private decimal currentBet;

        public GameEngine(string playerName, decimal startingBalance, int numberOfDecks = 1)
        {
            player = new Player(playerName, startingBalance);
            dealer = new Dealer("Dealer");
            deck = new Deck(numberOfDecks);
        }

        public void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Blackjack!");

            while (player.Balance > 0)
            {
                PlayRound();
                Console.WriteLine($"Your current balance: ${player.Balance}");

                Console.Write("Play another round? (y/n): ");
                string playAgain = Console.ReadLine()?.ToLower();
                if (playAgain != "y")
                {
                    break;
                }
                Console.Clear();
            }

            if (player.Balance <= 0)
            {
                Console.WriteLine("You're out of money! Game over.");
                Console.Write("Restart with initial balance? (y/n): ");
                string restart = Console.ReadLine()?.ToLower();
                if (restart == "y")
                {
                    player.Balance = 100; // Reset to initial balance
                    StartGame(); // Start a new game
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void PlayRound()
        {
            PlaceBet();

            // Deal initial hands
            player.ResetHand();
            dealer.ResetHand();

            player.Hand.AddCard(deck.Deal());
            dealer.Hand.AddCard(deck.Deal());
            player.Hand.AddCard(deck.Deal());
            dealer.Hand.AddCard(deck.Deal());

            DisplayHands(true);

            // Player's turn
            if (player.Hand.GetValue() == 21)
            {
                Console.WriteLine("Blackjack!");
            }
            else
            {
                PlayerTurn();
            }

            // Dealer's turn (if player didn't bust)
            if (player.Hand.GetValue() <= 21)
            {
                DealerTurn();
            }

            // Determine the winner
            DetermineWinner();
        }

        private void PlaceBet()
        {
            while (true)
            {
                Console.Write($"Enter your bet (Current balance: ${player.Balance}): ");
                if (decimal.TryParse(Console.ReadLine(), out currentBet))
                {
                    if (currentBet > 0 && currentBet <= player.Balance)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid bet. Please enter an amount between 0 and your current balance.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numeric value.");
                }
            }
        }

        private void PlayerTurn()
        {
            while (true)
            {
                Console.Write("Hit or Stand? (h/s): ");
                string action = Console.ReadLine()?.ToLower();

                if (action == "h")
                {
                    player.Hand.AddCard(deck.Deal());
                    DisplayHands(true);

                    if (player.Hand.GetValue() > 21)
                    {
                        Console.WriteLine("Bust!");
                        return;
                    }

                    if (player.Hand.GetValue() == 21)
                    {
                        Console.WriteLine("21!");
                        return;
                    }
                }
                else if (action == "s")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid action. Please enter 'h' or 's'.");
                }
            }
        }

        private void DealerTurn()
        {
            Console.WriteLine("\nDealer's Turn:");
            DisplayHands(false);

            while (dealer.ShouldHit())
            {
                dealer.Hand.AddCard(deck.Deal());
                Console.WriteLine("Dealer hits.");
                DisplayHands(false);

                if (dealer.Hand.GetValue() > 21)
                {
                    Console.WriteLine("Dealer busts!");
                    return;
                }
            }

            Console.WriteLine("Dealer stands.");
        }

        private void DetermineWinner()
        {
            int playerValue = player.Hand.GetValue();
            int dealerValue = dealer.Hand.GetValue();

            Console.WriteLine("\n--- Results ---");
            Console.WriteLine($"Your hand value: {playerValue}");
            Console.WriteLine($"Dealer's hand value: {dealerValue}");

            if (playerValue > 21)
            {
                Console.WriteLine("You bust! Dealer wins.");
                player.Balance -= currentBet;
            }
            else if (dealerValue > 21)
            {
                Console.WriteLine("Dealer busts! You win!");
                player.Balance += currentBet;
            }
            else if (playerValue > dealerValue)
            {
                Console.WriteLine("You win!");
                player.Balance += currentBet;
            }
            else if (dealerValue > playerValue)
            {
                Console.WriteLine("Dealer wins!");
                player.Balance -= currentBet;
            }
            else
            {
                Console.WriteLine("Push (tie).");
            }
        }

        private void DisplayHands(bool hideDealerCard)
        {
            Console.WriteLine("\n--- Hands ---");

            // Player's Hand
            Console.WriteLine($"Your hand: {string.Join(", ", player.Hand.Cards)}");
            Console.WriteLine($"Your hand value: {player.Hand.GetValue()}");

            // Dealer's Hand
            Console.Write("Dealer's hand: ");
            if (hideDealerCard)
            {
                Console.Write("[Hidden Card], ");
                for (int i = 1; i < dealer.Hand.Cards.Count; i++)
                {
                    Console.Write(dealer.Hand.Cards[i] + (i < dealer.Hand.Cards.Count - 1 ? ", " : ""));
                }
            }
            else
            {
                Console.Write(string.Join(", ", dealer.Hand.Cards));
            }
            Console.WriteLine();

            if (!hideDealerCard)
            {
                Console.WriteLine($"Dealer's hand value: {dealer.Hand.GetValue()}");
            }

            Console.WriteLine("-------------");
        }
    }
}