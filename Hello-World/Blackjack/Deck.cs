using System;
using System.Collections.Generic;

namespace Hello_World.Blackjack;

public sealed class Deck
{
    private readonly Random _random = new();
    private readonly int _deckCount;
    private Stack<Card> _cards = new();

    public Deck(int deckCount = 1)
    {
        if (deckCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(deckCount), "Deck count must be positive.");

        _deckCount = deckCount;
        InitializeAndShuffle();
    }

    private void InitializeAndShuffle()
    {
        var list = new List<Card>(_deckCount * 52);
        for (var d = 0; d < _deckCount; d++)
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    list.Add(new Card(suit, rank));
                }
            }
        }

        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        _cards = new Stack<Card>(list);
    }

    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            InitializeAndShuffle();
        }

        return _cards.Pop();
    }

    public void Reshuffle()
    {
        InitializeAndShuffle();
    }
}