using System.Text;

namespace Blackjack.Src;

public class GameEngine
{
    private readonly Deck _deck;
    private readonly Player _player;
    private readonly Dealer _dealer;
    private decimal _currentBet;

    public GameEngine()
    {
        _deck = new Deck();
        _player = new Player("Player", 100); // Starting balance $100
        _dealer = new Dealer();
        
        // Setup console encoding for card symbols if supported
        Console.OutputEncoding = Encoding.UTF8;
    }

    public void Run()
    {
        while (true)
        {
            if (_player.Balance <= 0)
            {
                Console.Clear();
                DisplayMessage("GAME OVER", ConsoleColor.Red);
                Console.WriteLine("You have run out of money.");
                Console.Write("Would you like to restart with $100? (y/n): ");
                var input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y")
                {
                    _player.Balance = 100;
                }
                else
                {
                    break;
                }
            }

            PlayRound();
            
            // Check deck threshold (shuffle if less than 15 cards)
            if (_deck.RemainingCards < 15)
            {
                DisplayMessage("Shuffling deck...", ConsoleColor.Yellow);
                _deck.Initialize();
                Thread.Sleep(1000);
            }
        }
        
        Console.WriteLine("Thanks for playing!");
    }

    private void PlayRound()
    {
        _player.ResetHand();
        _dealer.ResetHand();
        Console.Clear();

        // 1. Betting Phase
        DisplayHeader();
        if (!PlaceBet()) return; // Exit if bet cancelled or invalid logic

        // 2. Dealing Phase
        _player.Hand.AddCard(_deck.Draw());
        _dealer.Hand.AddCard(_deck.Draw());
        _player.Hand.AddCard(_deck.Draw());
        _dealer.Hand.AddCard(_deck.Draw());

        // 3. Check for Natural Blackjack
        if (_player.Hand.IsBlackjack)
        {
            DrawTable(revealDealer: true);
            DisplayMessage("BLACKJACK! You win!", ConsoleColor.Green);
            Payout(1.5m); // 3:2 payout
            WaitForNextRound();
            return;
        }

        // 4. Player Turn
        bool playerBusted = false;
        while (true)
        {
            DrawTable(revealDealer: false);
            
            if (_player.Hand.CalculateValue() == 21)
            {
                // Auto stand on 21
                break; 
            }

            Console.WriteLine("\nChoose action: [H]it or [S]tand");
            Console.Write("> ");
            var action = Console.ReadKey(intercept: true).Key;

            if (action == ConsoleKey.H)
            {
                Card card = _deck.Draw();
                _player.Hand.AddCard(card);
                DisplayMessage($"You drew: {card}", ConsoleColor.Yellow);
                Thread.Sleep(800); // visual pause

                if (_player.Hand.IsBusted)
                {
                    playerBusted = true;
                    break;
                }
            }
            else if (action == ConsoleKey.S)
            {
                break;
            }
        }

        // 5. Resolution
        DrawTable(revealDealer: true);

        if (playerBusted)
        {
            DisplayMessage("BUSTED! You lose.", ConsoleColor.Red);
            // Bet is already deducted
        }
        else
        {
            // Dealer Turn
            DisplayMessage("Dealer's turn...", ConsoleColor.Yellow);
            Thread.Sleep(1000);

            while (_dealer.ShouldHit())
            {
                Card card = _deck.Draw();
                _dealer.Hand.AddCard(card);
                DrawTable(revealDealer: true);
                Console.WriteLine($"\nDealer drew: {card}");
                Thread.Sleep(1000);
            }

            int playerVal = _player.Hand.CalculateValue();
            int dealerVal = _dealer.Hand.CalculateValue();

            if (_dealer.Hand.IsBusted)
            {
                DisplayMessage("Dealer Busted! You Win!", ConsoleColor.Green);
                Payout(1.0m); // 1:1
            }
            else if (playerVal > dealerVal)
            {
                DisplayMessage($"You Win! ({playerVal} vs {dealerVal})", ConsoleColor.Green);
                Payout(1.0m); // 1:1
            }
            else if (playerVal < dealerVal)
            {
                DisplayMessage($"Dealer Wins. ({dealerVal} vs {playerVal})", ConsoleColor.Red);
            }
            else
            {
                DisplayMessage($"Push (Tie). ({playerVal} vs {dealerVal})", ConsoleColor.Blue);
                Payout(0m, returnBet: true);
            }
        }

        WaitForNextRound();
    }

    private bool PlaceBet()
    {
        while (true)
        {
            Console.WriteLine($"Current Balance: ${_player.Balance}");
            Console.Write("Enter bet amount (or 'q' to quit): ");
            string? input = Console.ReadLine();

            if (input?.Trim().ToLower() == "q")
                Environment.Exit(0);

            if (decimal.TryParse(input, out decimal bet))
            {
                if (bet <= 0)
                {
                    DisplayMessage("Bet must be greater than 0.", ConsoleColor.Red);
                    continue;
                }
                if (bet > _player.Balance)
                {
                    DisplayMessage("Insufficient funds.", ConsoleColor.Red);
                    continue;
                }

                _currentBet = bet;
                _player.Balance -= bet; // Deduct immediately, refund on push/win
                return true;
            }
            
            DisplayMessage("Invalid input.", ConsoleColor.Red);
        }
    }

    private void Payout(decimal multiplier, bool returnBet = false)
    {
        decimal winnings = 0;
        if (returnBet)
        {
            winnings = _currentBet;
        }
        else
        {
            // Original bet + winnings
            winnings = _currentBet + (_currentBet * multiplier);
        }
        
        _player.Balance += winnings;
        Console.WriteLine($"Paid out: ${winnings:0.00}. New Balance: ${_player.Balance}");
    }

    private void DisplayHeader()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=== CONSOLE BLACKJACK ===");
        Console.ResetColor();
    }

    private void DisplayMessage(string msg, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    private void DrawTable(bool revealDealer)
    {
        Console.Clear();
        DisplayHeader();
        Console.WriteLine($"Balance: ${_player.Balance} | Current Bet: ${_currentBet}");
        Console.WriteLine(new string('-', 40));

        // Dealer Hand
        Console.Write("Dealer's Hand: ");
        if (revealDealer)
        {
            PrintHand(_dealer.Hand);
            Console.WriteLine($" (Total: {_dealer.Hand.CalculateValue()})");
        }
        else
        {
            // Show first card, hide second
            var firstCard = _dealer.Hand.Cards[0];
            PrintCard(firstCard);
            Console.Write(" [??]");
            Console.WriteLine(); // Hide Value
        }

        Console.WriteLine();

        // Player Hand
        Console.Write("Player's Hand: ");
        PrintHand(_player.Hand);
        Console.WriteLine($" (Total: {_player.Hand.CalculateValue()})");
        
        Console.WriteLine(new string('-', 40));
    }

    private void PrintHand(Hand hand)
    {
        foreach (var card in hand.Cards)
        {
            PrintCard(card);
            Console.Write(" ");
        }
    }

    private void PrintCard(Card card)
    {
        Console.ForegroundColor = card.GetColor();
        Console.Write(card.ToString());
        Console.ResetColor();
    }
    
    private void WaitForNextRound()
    {
        Console.WriteLine("\nPress any key for next round...");
        Console.ReadKey(true);
    }
}