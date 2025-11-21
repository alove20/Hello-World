using System;
using System.Threading;
using BlackjackGame.Models;
using BlackjackGame.UI;

namespace BlackjackGame
{
    public class GameEngine
    {
        private Player _player;
        private Dealer _dealer;
        private Deck _deck;
        private Display _display;
        private const decimal STARTING_BALANCE = 100m;

        public GameEngine()
        {
            _player = new Player(STARTING_BALANCE);
            _dealer = new Dealer();
            _deck = new Deck(1);
            _display = new Display();
        }

        public void Start()
        {
            _display.ShowWelcome();
            _deck.Shuffle();

            bool continuePlaying = true;

            while (continuePlaying && _player.HasMoney())
            {
                PlayRound();
                
                if (!_player.HasMoney())
                {
                    _display.ShowGameOver();
                    if (_display.AskToRestart())
                    {
                        _player = new Player(STARTING_BALANCE);
                        _deck.Reset();
                    }
                    else
                    {
                        continuePlaying = false;
                    }
                }
                else
                {
                    continuePlaying = _display.AskToContinue();
                }
            }

            _display.ShowGoodbye();
        }

        private void PlayRound()
        {
            // Reset hands
            _player.ResetHand();
            _dealer.ResetHand();

            // Reshuffle if deck is running low
            if (_deck.CardsRemaining < 15)
            {
                _display.ShowMessage("Shuffling new deck...", ConsoleColor.Yellow);
                Thread.Sleep(1000);
                _deck.Reset();
            }

            // Place bet
            _display.ShowBalance(_player.Balance);
            decimal bet = _display.GetBet(_player.Balance);
            
            if (!_player.PlaceBet(bet))
            {
                _display.ShowMessage("Invalid bet amount!", ConsoleColor.Red);
                return;
            }

            // Initial deal
            _player.Hand.AddCard(_deck.DrawCard());
            _dealer.Hand.AddCard(_deck.DrawCard());
            _player.Hand.AddCard(_deck.DrawCard());
            
            var dealerSecondCard = _deck.DrawCard();
            dealerSecondCard.IsFaceDown = true;
            _dealer.Hand.AddCard(dealerSecondCard);

            // Display initial hands
            _display.ShowGameState(_player, _dealer, true);

            // Check for player blackjack
            if (_player.Hand.IsBlackjack())
            {
                dealerSecondCard.IsFaceDown = false;
                _display.ShowGameState(_player, _dealer, false);
                
                if (_dealer.Hand.IsBlackjack())
                {
                    _display.ShowMessage("Both have Blackjack! Push!", ConsoleColor.Yellow);
                    _player.Push();
                }
                else
                {
                    _display.ShowMessage("BLACKJACK! You win!", ConsoleColor.Green);
                    _player.Win(1.5m); // 3:2 payout
                }
                return;
            }

            // Player's turn
            bool playerStanding = false;
            while (!_player.Hand.IsBusted() && !playerStanding)
            {
                var action = _display.GetPlayerAction();
                
                switch (action)
                {
                    case PlayerAction.Hit:
                        _player.Hand.AddCard(_deck.DrawCard());
                        _display.ShowGameState(_player, _dealer, true);
                        
                        if (_player.Hand.IsBusted())
                        {
                            _display.ShowMessage("BUST! You lose!", ConsoleColor.Red);
                            _player.Lose();
                            return;
                        }
                        break;
                    
                    case PlayerAction.Stand:
                        playerStanding = true;
                        break;
                }
            }

            // Reveal dealer's hidden card
            dealerSecondCard.IsFaceDown = false;
            _display.ShowMessage("Dealer reveals hidden card...", ConsoleColor.Cyan);
            Thread.Sleep(1000);
            _display.ShowGameState(_player, _dealer, false);
            Thread.Sleep(1000);

            // Dealer's turn
            while (_dealer.ShouldHit())
            {
                _display.ShowMessage("Dealer hits...", ConsoleColor.Cyan);
                Thread.Sleep(1000);
                _dealer.Hand.AddCard(_deck.DrawCard());
                _display.ShowGameState(_player, _dealer, false);
                Thread.Sleep(1000);

                if (_dealer.Hand.IsBusted())
                {
                    _display.ShowMessage("Dealer busts! You win!", ConsoleColor.Green);
                    _player.Win();
                    return;
                }
            }

            _display.ShowMessage("Dealer stands.", ConsoleColor.Cyan);
            Thread.Sleep(1000);

            // Determine winner
            int playerValue = _player.Hand.GetValue();
            int dealerValue = _dealer.Hand.GetValue();

            if (playerValue > dealerValue)
            {
                _display.ShowMessage($"You win! ({playerValue} vs {dealerValue})", ConsoleColor.Green);
                _player.Win();
            }
            else if (playerValue < dealerValue)
            {
                _display.ShowMessage($"Dealer wins! ({dealerValue} vs {playerValue})", ConsoleColor.Red);
                _player.Lose();
            }
            else
            {
                _display.ShowMessage($"Push! ({playerValue} vs {dealerValue})", ConsoleColor.Yellow);
                _player.Push();
            }
        }
    }
}