namespace Blackjack.Src;

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int Value
    {
        get
        {
            if (Rank >= Rank.Two && Rank <= Rank.Ten)
                return (int)Rank;
            if (Rank == Rank.Jack || Rank == Rank.Queen || Rank == Rank.King)
                return 10;
            if (Rank == Rank.Ace)
                return 11;
            return 0;
        }
    }

    public override string ToString()
    {
        string rankDisplay = Rank switch
        {
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            Rank.Ace => "A",
            _ => ((int)Rank).ToString()
        };

        string suitSymbol = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };

        return $"{rankDisplay}{suitSymbol}";
    }
    
    // Helper to get color for console display based on suit
    public ConsoleColor GetColor()
    {
        return (Suit == Suit.Hearts || Suit == Suit.Diamonds) ? ConsoleColor.Red : ConsoleColor.Cyan;
    }
}