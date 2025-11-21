namespace BlackjackApp.Models;

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

    public int CalculateScore()
    {
        int score = 0;
        int aceCount = 0;

        foreach (var card in Cards)
        {
            score += card.GetBlackjackValue();
            if (card.Rank == Rank.Ace)
            {
                aceCount++;
            }
        }

        // Adjust for Aces if bust
        while (score > 21 && aceCount > 0)
        {
            score -= 10; // Treat Ace as 1 instead of 11
            aceCount--;
        }

        return score;
    }

    public bool IsBlackjack()
    {
        return Cards.Count == 2 && CalculateScore() == 21;
    }

    public bool IsBusted()
    {
        return CalculateScore() > 21;
    }
}