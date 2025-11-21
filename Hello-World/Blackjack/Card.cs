using System;

namespace Hello_World.Blackjack;

public enum Suit
{
    Clubs,
    Diamonds,
    Hearts,
    Spades
}

public enum Rank
{
    Two = 2,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}

public sealed class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }

    public Card(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public int GetValue()
    {
        return Rank switch
        {
            Rank.Jack or Rank.Queen or Rank.King => 10,
            Rank.Ace => 11, // Aces handled specially in Hand
            _ => (int)Rank
        };
    }

    public ConsoleColor GetSuitColor()
    {
        return Suit switch
        {
            Suit.Hearts or Suit.Diamonds => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    public string GetSuitSymbol()
    {
        return Suit switch
        {
            Suit.Clubs => "♣",
            Suit.Diamonds => "♦",
            Suit.Hearts => "♥",
            Suit.Spades => "♠",
            _ => "?"
        };
    }

    public string GetRankString()
    {
        return Rank switch
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
    }

    public override string ToString()
    {
        return $"{GetRankString()}{GetSuitSymbol()}";
    }
}