using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame
{
    public class Deck
    {
        private List<Card> cards;
        private Random random;

        public Deck()
        {
            random = new Random();
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            cards = new List<Card>();

            var suits = new[] { Suit.Hearts, Suit.Diamonds, Suit.Clubs, Suit.Spades };
            var ranks = new[]
            {
                (Rank.Two, "2"),
                (Rank.Three, "3"),
                (Rank.Four, "4"),
                (Rank.Five, "5"),
                (Rank.Six, "6"),
                (Rank.Seven, "7"),
                (Rank.Eight, "8"),
                (Rank.Nine, "9"),
                (Rank.Ten, "10"),
                (Rank.Jack, "J"),
                (Rank.Queen, "Q"),
                (Rank.King, "K"),
                (Rank.Ace, "A")
            };

            foreach (var suit in suits)
            {
                foreach (var (rank, rankName) in ranks)
                {
                    cards.Add(new Card(suit, rank, rankName));
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            cards = cards.OrderBy(x => random.Next()).ToList();
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                InitializeDeck();
            }

            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int RemainingCards()
        {
            return cards.Count;
        }
    }
}