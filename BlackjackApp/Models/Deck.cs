namespace BlackjackApp.Models;

public class Deck
{
    private Stack<Card> _cards = new();
    private readonly Random _rng = new();

    public Deck()
    {
        InitializeDeck();
    }

    private void InitializeDeck()
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
    }

    private void Shuffle(List<Card> cardsList)
    {
        // Fisher-Yates shuffle
        int n = cardsList.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            (cardsList[k], cardsList[n]) = (cardsList[n], cardsList[k]);
        }

        _cards = new Stack<Card>(cardsList);
    }

    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            InitializeDeck(); // Fallback if deck runs out mid-game
        }
        return _cards.Pop();
    }

    public void ReshuffleIfLow()
    {
        // Reshuffle if less than 15 cards remain
        if (_cards.Count < 15) 
        {
            InitializeDeck();
        }
    }
}