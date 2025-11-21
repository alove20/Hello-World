namespace BlackjackApp.Models;

public class Hand
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

    public int CalculateScore()
    {
        int score = 0;
        int aceCount = 0;

        foreach (var card in _cards)
        {
            if (card.Rank == Rank.Ace)
            {
                aceCount++;
                score += 11;
            }
            else
            {
                score += card.Value;
            }
        }

        // Adjust for Aces if we are over 21
        while (score > 21 && aceCount > 0)
        {
            score -= 10; // Convert an Ace from 11 to 1
            aceCount--;
        }

        return score;
    }

    public bool IsBlackjack => _cards.Count == 2 && CalculateScore() == 21;
    public bool IsBusted => CalculateScore() > 21;
}