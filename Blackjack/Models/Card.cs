namespace Blackjack.Models;

public record Card(Rank Rank, Suit Suit)
{
    public int GetValue()
    {
        if (Rank >= Rank.Two && Rank <= Rank.Ten)
        {
            return (int)Rank;
        }
        
        if (Rank == Rank.Jack || Rank == Rank.Queen || Rank == Rank.King)
        {
            return 10;
        }

        // Ace handling is context-dependent, returning 11 default
        if (Rank == Rank.Ace)
        {
            return 11;
        }

        return 0;
    }

    public override string ToString()
    {
        var rankSymbol = Rank switch
        {
            Rank.Two => "2",
            Rank.Three => "3",
            Rank.Four => "4",
            Rank.Five => "5",
            Rank.Six => "6",
            Rank.Seven => "7",
            Rank.Eight => "8",
            Rank.Nine => "9",
            Rank.Ten => "10",
            Rank.Jack => "J",
            Rank.Queen => "Q",
            Rank.King => "K",
            Rank.Ace => "A",
            _ => "?"
        };

        var suitSymbol = Suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };

        return $"{rankSymbol}{suitSymbol}";
    }
}