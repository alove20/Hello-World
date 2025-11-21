namespace BlackjackApp.Models;

public class Deck
{
    private Stack<Card> _cards = new();

    public Deck()
    {
        Initialize();
    }

    public void Initialize()
    {
        var cardsList = new List<Card>();
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                cardsList.Add(new Card(suit, rank));
            }
        }
        
        Shuffle(cardsList);
        
        _cards = new Stack<Card>(cardsList);
    }

    private void Shuffle(List<Card> cards)
    {
        // Fisher-Yates shuffle
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Shared.Next(n + 1);
            (cards[k], cards[n]) = (cards[n], cards[k]);
        }
    }

    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            // Simple approach: If deck runs out, re-shuffle a fresh deck
            // In a real casino, this is complex, but acceptable for this scope.
            Initialize();
        }
        return _cards.Pop();
    }
}