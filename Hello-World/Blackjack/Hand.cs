using System.Collections.Generic;
using System.Linq;

namespace Hello_World.Blackjack;

public sealed class Hand
{
    private readonly List<Card> _cards = new();

    public IReadOnlyList<Card> Cards => _cards;

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }

    public void Clear()
    {
        _cards.Clear();
    }

    public int GetBestValue()
    {
        var total = 0;
        var aceCount = 0;

        foreach (var card in _cards)
        {
            total += card.GetValue();
            if (card.Rank == Rank.Ace)
                aceCount++;
        }

        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }

    public bool IsBlackjack()
    {
        return _cards.Count == 2 && GetBestValue() == 21;
    }

    public bool IsBust()
    {
        return GetBestValue() > 21;
    }

    public override string ToString()
    {
        return string.Join(" ", _cards.Select(c => c.ToString()));
    }
}