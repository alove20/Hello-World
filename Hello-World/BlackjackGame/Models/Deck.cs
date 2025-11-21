using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame.Models
{
    public class Deck
    {
        private List<Card> cards;
        private Random random;

        public Deck(int numberOfDecks = 1)
        {
            random = new Random();
            cards = new List<Card>();
            InitializeDeck(numberOfDecks);
        }

        private void InitializeDeck(int numberOfDecks)
        {
            cards.Clear();
            
            for (int d = 0; d < numberOfDecks; d++)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        cards.Add(new Card(suit, rank));
                    }
                }
            }
            
            Shuffle();
        }

        public void Shuffle()
        {
            // Fisher-Yates shuffle algorithm
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
            {
                InitializeDeck(1); // Reshuffle if deck is empty
            }

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int RemainingCards => cards.Count;

        public void Reset(int numberOfDecks = 1)
        {
            InitializeDeck(numberOfDecks);
        }
    }
}