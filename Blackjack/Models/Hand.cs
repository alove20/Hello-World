using System.Text;

namespace Blackjack.Models;

public class Hand
{
    public List<Card> Cards { get; } = new();

    public void AddCard(Card card)
    {
        Cards.Add(card);
    }

    public void Clear()
    {
        Cards.Clear();
    }

    public int CalculateValue()
    {
        int value = 0;
        int aceCount = 0;

        foreach (var card in Cards)
        {
            if (card.Rank == Rank.Ace)
            {
                aceCount++;
                value += 11;
            }
            else
            {
                value += card.GetValue();
            }
        }

        // Adjust for Aces if busting
        while (value > 21 && aceCount > 0)
        {
            value -= 10;
            aceCount--;
        }

        return value;
    }

    public bool IsBlackjack()
    {
        return Cards.Count == 2 && CalculateValue() == 21;
    }

    public bool IsBusted()
    {
        return CalculateValue() > 21;
    }
}