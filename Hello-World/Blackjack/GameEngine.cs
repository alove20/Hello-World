using System;
using System.Threading;
using Blackjack.Models;
using Blackjack.UI;

namespace Blackjack
{
    public class GameEngine
    {
        private Deck _deck;
        private Player _player;
        private Dealer _dealer;
        private Display _display;
        private const decimal STARTING_BALANCE = 100m;

        public GameEngine()
        {
            _deck = new Deck(1);
            _player = new Player(STARTING_BALANCE);
            _dealer = new Dealer();
            _display = new Display();
        }

        public void Start()
        {
            _display.ShowWelcome();
            
            bool playing = true;
            while (playing)
            {
                if (!_player.HasMoney)
                {
                    _display.ShowGameOver();
                    if (_display.AskPlayAgain())
                    {
                        _player = new Player(STARTING_BALANCE);
                        _deck.Reset();
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                PlayRound();

                if (_player.HasMoney)
                {
                    if (!_display.AskPlayAgain())
                    {
                        playing = false;
                    }
                }
            }

            _display.ShowGoodbye(_player.Balance);
        }

        private void PlayRound()
        {
            // Reset hands
            _player.ResetHand();
            _dealer.ResetHand();

            // Check if deck needs reshuffling
            if (_deck.CardsRemaining < 15)
            {
                _display.ShowMessage("Reshuffling deck...", ConsoleColor.Yellow);
                Thread.Sleep(1000);
                _deck.Reset();
            }

            // Place bet
            decimal bet = _display.GetBet(_player.Balance);
            if (!_player.PlaceBet(bet))
            {
                _display.ShowError("Invalid bet amount!");
                return;
            }

            // Deal initial cards
            _player.Hand.AddCard(_deck.DrawCard());
            _dealer.Hand.AddCard(_deck.DrawCard());
            _player.Hand.AddCard(_deck.DrawCard());
            
            var dealerSecondCard = _deck.DrawCard();
            dealerSecondCard.IsFaceDown = true;
            _dealer.Hand.AddCard(dealerSecondCard);

            // Display initial hands
            _display.ShowGameState(_player, _dealer, bet);

            // Check for blackjacks
            if (_player.Hand.IsBlackjack)
            {
                _dealer.RevealHiddenCard();
                _display.ShowGameState(_player, _dealer, bet);
                
                if (_dealer.Hand.IsBlackjack)
                {
                    _display.ShowResult("Both have Blackjack! Push!", ConsoleColor.Yellow);
                    _player.Push();
                }
                else
                {
                    _display.ShowResult("Blackjack! You win!", ConsoleColor.Green);
                    _player.Win(1.5m); // Blackjack pays 3:2
                }
                return;
            }

            if (_dealer.Hand.IsBlackjack)
            {
                _dealer.RevealHiddenCard();
                _display.ShowGameState(_player, _dealer, bet);
                _display.ShowResult("Dealer has Blackjack! Dealer wins!", ConsoleColor.Red);
                _player.Lose();
                return;
            }

            // Player's turn
            bool playerStanding = false;
            while (!_player.Hand.IsBusted && !playerStanding)
            {
                var action = _display.GetPlayerAction();
                
                if (action == PlayerAction.Hit)
                {
                    _player.Hand.AddCard(_deck.DrawCard());
                    _display.ShowGameState(_player, _dealer, bet);
                    
                    if (_player.Hand.IsBusted)
                    {
                        _display.ShowResult("Bust! Dealer wins!", ConsoleColor.Red);
                        _player.Lose();
                        return;
                    }
                }
                else if (action == PlayerAction.Stand)
                {
                    playerStanding = true;
                }
            }

            // Dealer's turn
            _dealer.RevealHiddenCard();
            _display.ShowGameState(_player, _dealer, bet);
            _display.ShowMessage("Dealer's turn...", ConsoleColor.Cyan);
            Thread.Sleep(1000);

            while (_dealer.ShouldHit())
            {
                _dealer.Hand.AddCard(_deck.DrawCard());
                _display.ShowGameState(_player, _dealer, bet);
                _display.ShowMessage("Dealer hits...", ConsoleColor.Cyan);
                Thread.Sleep(1000);

                if (_dealer.Hand.IsBusted)
                {
                    _display.ShowResult("Dealer busts! You win!", ConsoleColor.Green);
                    _player.Win();
                    return;
                }
            }

            // Compare hands
            int playerValue = _player.Hand.GetValue();
            int dealerValue = _dealer.Hand.GetValue();

            if (playerValue > dealerValue)
            {
                _display.ShowResult($"You win! ({playerValue} vs {dealerValue})", ConsoleColor.Green);
                _player.Win();
            }
            else if (dealerValue > playerValue)
            {
                _display.ShowResult($"Dealer wins! ({dealerValue} vs {playerValue})", ConsoleColor.Red);
                _player.Lose();
            }
            else
            {
                _display.ShowResult($"Push! ({playerValue} vs {dealerValue})", ConsoleColor.Yellow);
                _player.Push();
            }
        }
    }
}