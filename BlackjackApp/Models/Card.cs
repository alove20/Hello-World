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

    public int GetBlackjackValue()
    {
        if (Rank == Rank.Ace) return 11; // Hand logic will handle reduction to 1
        if ((int)Rank >= 10) return 10;  // Face cards are 10
        return (int)Rank;                // Number cards
    }

    public override string ToString()
    {
        string rankDisplay = Rank switch
        {
            Rank.Ace => "A",
            Rank.King => "K",
            Rank.Queen => "Q",
            Rank.Jack => "J",
            _ => ((int)Rank).ToString()
        };

        string suitDisplay = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };

        return $"{rankDisplay}{suitDisplay}";
    }

    public ConsoleColor GetColor()
    {
        return (Suit == Suit.Hearts || Suit == Suit.Diamonds) 
            ? ConsoleColor.Red 
            : ConsoleColor.Black;
    }
}