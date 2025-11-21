using System;
using System.Threading;
using Blackjack.Models;
using Blackjack.UI;

namespace Blackjack
{
    public class GameEngine
    {
        private const decimal STARTING_BALANCE = 100m;
        private Player player;
        private Dealer dealer;
        private Deck deck;
        private Display display;

        public GameEngine()
        {
            player = new Player(STARTING_BALANCE);
            dealer = new Dealer();
            deck = new Deck();
            display = new Display();
        }

        public void Start()
        {
            display.ShowWelcome();
            
            bool playing = true;
            while (playing)
            {
                if (!player.HasMoney())
                {
                    display.ShowGameOver();
                    if (display.AskPlayAgain())
                    {
                        player = new Player(STARTING_BALANCE);
                        deck.Reset();
                    }
                    else
                    {
                        playing = false;
                    }
                    continue;
                }

                PlayRound();

                if (player.HasMoney())
                {
                    if (!display.AskContinue())
                    {
                        playing = false;
                    }
                }
            }

            display.ShowGoodbye(player.Balance);
        }

        private void PlayRound()
        {
            // Clear previous hands
            player.ClearHand();
            dealer.ClearHand();

            // Show balance and get bet
            display.ShowBalance(player.Balance);
            decimal bet = display.GetBet(player.Balance);

            if (!player.PlaceBet(bet))
            {
                display.ShowError("Invalid bet amount!");
                return;
            }

            // Deal initial cards
            player.Hand.AddCard(deck.DrawCard());
            dealer.Hand.AddCard(deck.DrawCard());
            player.Hand.AddCard(deck.DrawCard());
            dealer.Hand.AddCard(deck.DrawCard());

            // Show initial hands
            display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: true);

            // Check for blackjacks
            if (player.Hand.IsBlackjack() && dealer.Hand.IsBlackjack())
            {
                display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: false);
                display.ShowPush();
                player.Win(bet); // Return bet
                return;
            }
            else if (player.Hand.IsBlackjack())
            {
                display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: false);
                display.ShowBlackjack();
                player.Win(bet * 2.5m); // 3:2 payout + original bet
                return;
            }
            else if (dealer.Hand.IsBlackjack())
            {
                display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: false);
                display.ShowDealerBlackjack();
                return;
            }

            // Player's turn
            bool playerStanding = false;
            while (!playerStanding && !player.Hand.IsBusted())
            {
                string action = display.GetPlayerAction();
                
                if (action == "h")
                {
                    player.Hand.AddCard(deck.DrawCard());
                    display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: true);
                    
                    if (player.Hand.IsBusted())
                    {
                        display.ShowPlayerBust();
                        return;
                    }
                }
                else if (action == "s")
                {
                    playerStanding = true;
                }
            }

            // Reveal dealer's card
            display.ShowDealerReveal();
            Thread.Sleep(1000);
            display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: false);
            Thread.Sleep(1000);

            // Dealer's turn
            while (dealer.ShouldHit())
            {
                display.ShowDealerHits();
                Thread.Sleep(1000);
                dealer.Hand.AddCard(deck.DrawCard());
                display.ShowHands(player.Hand, dealer.Hand, hideFirstDealerCard: false);
                Thread.Sleep(1000);

                if (dealer.Hand.IsBusted())
                {
                    display.ShowDealerBust();
                    player.Win(bet * 2); // Return bet + winnings
                    return;
                }
            }

            display.ShowDealerStands();
            Thread.Sleep(1000);

            // Determine winner
            DetermineWinner(bet);
        }

        private void DetermineWinner(decimal bet)
        {
            int playerValue = player.Hand.GetValue();
            int dealerValue = dealer.Hand.GetValue();

            if (playerValue > dealerValue)
            {
                display.ShowPlayerWins(playerValue, dealerValue);
                player.Win(bet * 2); // Return bet + winnings
            }
            else if (dealerValue > playerValue)
            {
                display.ShowDealerWins(playerValue, dealerValue);
            }
            else
            {
                display.ShowPush();
                player.Win(bet); // Return bet
            }
        }
    }
}