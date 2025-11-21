using System;

namespace Hello_World.Blackjack;

public sealed class GameEngine
{
    private const decimal StartingBalance = 100m;
    private readonly ConsoleUi _ui;
    private Deck _deck;
    private Player _player = null!;
    private Dealer _dealer = null!;

    public GameEngine()
    {
        _ui = new ConsoleUi();
        _deck = new Deck(1);
    }

    public void Run()
    {
        InitializeGame();

        var exit = false;
        while (!exit)
        {
            if (_player.Balance <= 0)
            {
                if (_ui.AskRestart())
                {
                    _player = new Player(_player.Name, StartingBalance);
                    continue;
                }

                break;
            }

            _ui.Clear();
            _ui.WriteBanner();
            _ui.ShowBalances(_player);

            var bet = _ui.AskForBet(_player);
            if (bet == 0)
                break;

            PlayHand(bet);

            if (_player.Balance <= 0)
                continue;

            if (!_ui.AskPlayAgain())
                exit = true;
        }

        Console.WriteLine();
        Console.WriteLine("Thanks for playing Blackjack!");
    }

    private void InitializeGame()
    {
        _ui.WriteBanner();
        Console.Write("Enter your name (or leave blank for 'Player'): ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
            name = "Player";

        _player = new Player(name, StartingBalance);
        _dealer = new Dealer();
    }

    private void PlayHand(decimal bet)
    {
        _player.Hand.Clear();
        _dealer.Hand.Clear();
        _deck.Reshuffle();

        DealInitialCards();

        var playerHasBlackjack = _player.Hand.IsBlackjack();
        var dealerHasBlackjack = _dealer.Hand.IsBlackjack();

        _ui.Clear();
        _ui.WriteBanner();
        _ui.ShowBalances(_player);
        Console.WriteLine($"Current bet: {bet:C}");
        _ui.ShowHands(_player, _dealer, hideDealerHoleCard: true);

        if (playerHasBlackjack || dealerHasBlackjack)
        {
            ResolveBlackjack(bet, playerHasBlackjack, dealerHasBlackjack);
            _ui.PauseForUser();
            return;
        }

        PlayerTurn();

        if (_player.Hand.IsBust())
        {
            _ui.ShowHands(_player, _dealer, hideDealerHoleCard: false);
            _player.ApplyLoss(bet);
            _ui.AnnounceResult("You bust! Dealer wins.", ConsoleColor.Red);
            _ui.PauseForUser();
            return;
        }

        DealerTurn();

        _ui.ShowHands(_player, _dealer, hideDealerHoleCard: false);
        ResolveWinner(bet);
        _ui.PauseForUser();
    }

    private void DealInitialCards()
    {
        _player.Hand.AddCard(_deck.Draw());
        _dealer.Hand.AddCard(_deck.Draw());
        _player.Hand.AddCard(_deck.Draw());
        _dealer.Hand.AddCard(_deck.Draw());
    }

    private void PlayerTurn()
    {
        while (true)
        {
            _ui.ShowHands(_player, _dealer, hideDealerHoleCard: true);

            if (_player.Hand.IsBust())
                break;

            var action = _ui.AskAction();

            if (action == "stand")
                break;

            if (action == "hit")
            {
                _player.Hand.AddCard(_deck.Draw());
                Console.WriteLine();
            }
        }
    }

    private void DealerTurn()
    {
        while (_dealer.ShouldHit())
        {
            _dealer.Hand.AddCard(_deck.Draw());
        }
    }

    private void ResolveBlackjack(decimal bet, bool playerHasBlackjack, bool dealerHasBlackjack)
    {
        _ui.ShowHands(_player, _dealer, hideDealerHoleCard: false);

        if (playerHasBlackjack && dealerHasBlackjack)
        {
            _ui.AnnounceResult("Both you and the dealer have Blackjack. Push!", ConsoleColor.Yellow);
            return;
        }

        if (playerHasBlackjack)
        {
            // Simple 1:1 payout as requested
            _player.ApplyWin(bet);
            _ui.AnnounceResult("Blackjack! You win!", ConsoleColor.Green);
            return;
        }

        _player.ApplyLoss(bet);
        _ui.AnnounceResult("Dealer has Blackjack. You lose.", ConsoleColor.Red);
    }

    private void ResolveWinner(decimal bet)
    {
        var playerTotal = _player.Hand.GetBestValue();
        var dealerTotal = _dealer.Hand.GetBestValue();

        if (_dealer.Hand.IsBust())
        {
            _player.ApplyWin(bet);
            _ui.AnnounceResult("Dealer busts! You win!", ConsoleColor.Green);
            return;
        }

        if (playerTotal > dealerTotal)
        {
            _player.ApplyWin(bet);
            _ui.AnnounceResult("You win!", ConsoleColor.Green);
            return;
        }

        if (playerTotal < dealerTotal)
        {
            _player.ApplyLoss(bet);
            _ui.AnnounceResult("Dealer wins.", ConsoleColor.Red);
            return;
        }

        _ui.AnnounceResult("Push. Your bet is returned.", ConsoleColor.Yellow);
    }
}