namespace Blackjack.Src;

public class Hand
{
    private readonly List<Card> _cards = new();

    public IReadOnlyList<Card> Cards => _cards.AsReadOnly();

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }

    public void Clear()
    {
        _cards.Clear();
    }

    public int CalculateValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (var card in _cards)
        {
            value += card.Value;
            if (card.Rank == Rank.Ace)
            {
                aceCount++;
            }
        }

        // Adjust for Aces if we are over 21
        while (value > 21 && aceCount > 0)
        {
            value -= 10; // Treat Ace as 1 instead of 11
            aceCount--;
        }

        return value;
    }

    public bool IsBlackjack => _cards.Count == 2 && CalculateValue() == 21;
    public bool IsBusted => CalculateValue() > 21;
}