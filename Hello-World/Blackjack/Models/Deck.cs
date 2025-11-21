using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack.Models
{
    public class Deck
    {
        private List<Card> cards;
        private Random random;

        public Deck()
        {
            random = new Random();
            cards = new List<Card>();
            InitializeDeck();
            Shuffle();
        }

        private void InitializeDeck()
        {
            cards.Clear();
            var suits = new[] { Suit.Hearts, Suit.Diamonds, Suit.Clubs, Suit.Spades };
            
            foreach (var suit in suits)
            {
                cards.Add(new Card(suit, Rank.Two, "2"));
                cards.Add(new Card(suit, Rank.Three, "3"));
                cards.Add(new Card(suit, Rank.Four, "4"));
                cards.Add(new Card(suit, Rank.Five, "5"));
                cards.Add(new Card(suit, Rank.Six, "6"));
                cards.Add(new Card(suit, Rank.Seven, "7"));
                cards.Add(new Card(suit, Rank.Eight, "8"));
                cards.Add(new Card(suit, Rank.Nine, "9"));
                cards.Add(new Card(suit, Rank.Ten, "10"));
                cards.Add(new Card(suit, Rank.Jack, "J"));
                cards.Add(new Card(suit, Rank.Queen, "Q"));
                cards.Add(new Card(suit, Rank.King, "K"));
                cards.Add(new Card(suit, Rank.Ace, "A"));
            }
        }

        public void Shuffle()
        {
            // Fisher-Yates shuffle algorithm
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card temp = cards[k];
                cards[k] = cards[n];
                cards[n] = temp;
            }
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                InitializeDeck();
                Shuffle();
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining()
        {
            return cards.Count;
        }

        public void Reset()
        {
            InitializeDeck();
            Shuffle();
        }
    }
}