namespace BlackjackApp.Models;

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    /// <summary>
    /// Returns the basic blackjack value. Aces return 11 by default.
    /// Logic for 1 vs 11 is handled in Hand.cs.
    /// </summary>
    public int Value
    {
        get
        {
            if (Rank >= Rank.Two && Rank <= Rank.Ten)
                return (int)Rank;
            if (Rank == Rank.Jack || Rank == Rank.Queen || Rank == Rank.King)
                return 10;
            return 11; // Ace
        }
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}