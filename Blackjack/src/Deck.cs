namespace Blackjack.Src;

public class Deck
{
    private readonly List<Card> _cards = new();
    private readonly Random _random = new();

    public Deck()
    {
        Initialize();
    }

    public void Initialize()
    {
        _cards.Clear();
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                _cards.Add(new Card(suit, rank));
            }
        }
        Shuffle();
    }

    public void Shuffle()
    {
        // Fisher-Yates shuffle
        int n = _cards.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
        }
    }

    public Card Draw()
    {
        if (_cards.Count == 0)
        {
            // Auto reshuffle if empty (simplified rule)
            Initialize();
        }

        var card = _cards[0];
        _cards.RemoveAt(0);
        return card;
    }
    
    public int RemainingCards => _cards.Count;
}