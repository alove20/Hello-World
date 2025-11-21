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
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }

    public class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public bool IsFaceDown { get; set; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
            IsFaceDown = false;
        }

        public int GetValue()
        {
            return Rank switch
            {
                Rank.Ace => 11,
                Rank.Jack or Rank.Queen or Rank.King => 10,
                _ => (int)Rank
            };
        }

        public bool IsAce => Rank == Rank.Ace;

        public override string ToString()
        {
            if (IsFaceDown)
            {
                return "[HIDDEN]";
            }

            string rankStr = Rank switch
            {
                Rank.Ace => "A",
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                _ => ((int)Rank).ToString()
            };

            string suitStr = Suit switch
            {
                Suit.Hearts => "♥",
                Suit.Diamonds => "♦",
                Suit.Clubs => "♣",
                Suit.Spades => "♠",
                _ => ""
            };

            return $"{rankStr}{suitStr}";
        }

        public ConsoleColor GetSuitColor()
        {
            return Suit == Suit.Hearts || Suit == Suit.Diamonds
                ? ConsoleColor.Red
                : ConsoleColor.White;
        }
    }
}