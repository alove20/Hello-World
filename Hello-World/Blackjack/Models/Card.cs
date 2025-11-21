using System;

namespace Blackjack.Models
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
    }

    public enum Rank
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 10,
        Queen = 10,
        King = 10,
        Ace = 11
    }

    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public string DisplayRank { get; }

        public Card(Suit suit, Rank rank, string displayRank)
        {
            Suit = suit;
            Rank = rank;
            DisplayRank = displayRank;
        }

        public int GetValue()
        {
            return (int)Rank;
        }

        public bool IsAce()
        {
            return DisplayRank == "A";
        }

        public override string ToString()
        {
            string suitSymbol = Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => ""
            };

            ConsoleColor suitColor = (Suit == Suit.Hearts || Suit == Suit.Diamonds) 
                ? ConsoleColor.Red 
                : ConsoleColor.White;

            return $"{DisplayRank}{suitSymbol}";
        }

        public ConsoleColor GetSuitColor()
        {
            return (Suit == Suit.Hearts || Suit == Suit.Diamonds) 
                ? ConsoleColor.Red 
                : ConsoleColor.White;
        }
    }
}