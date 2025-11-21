namespace Blackjack
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

    public class Card
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
            if ((int)Rank >= 10 && (int)Rank <= 13) // Jack, Queen, King
            {
                return 10;
            }
            else if (Rank == Rank.Ace)
            {
                return 11; // Ace is initially 11
            }
            else
            {
                return (int)Rank;
            }
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}