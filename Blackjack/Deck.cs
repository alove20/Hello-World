using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack
{
    public class Deck
    {
        private List<Card> cards;
        private readonly Random random = new Random();

        public Deck(int numberOfDecks = 1)
        {
            cards = new List<Card>();
            for (int i = 0; i < numberOfDecks; i++)
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
            cards = cards.OrderBy(x => random.Next()).ToList();
        }

        public Card Deal()
        {
            if (cards.Count == 0)
            {
                // Reshuffle if the deck is empty (should not happen often with one deck)
                Console.WriteLine("Reshuffling the deck.");
                Deck newDeck = new Deck(); // Creates a new unshuffled deck
                this.cards = newDeck.cards; // Assigns cards from the new deck
                Shuffle(); // Shuffle the new deck
            }
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining()
        {
            return cards.Count;
        }
    }
}