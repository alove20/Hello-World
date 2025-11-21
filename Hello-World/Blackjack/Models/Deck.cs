using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack.Models
{
    public class Deck
    {
        private List<Card> _cards;
        private Random _random;

        public Deck(int numberOfDecks = 1)
        {
            _cards = new List<Card>();
            _random = new Random();
            InitializeDeck(numberOfDecks);
        }

        private void InitializeDeck(int numberOfDecks)
        {
            _cards.Clear();
            
            for (int i = 0; i < numberOfDecks; i++)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        _cards.Add(new Card(suit, rank));
                    }
                }
            }
        }

        public void Shuffle()
        {
            var shuffled = _cards.OrderBy(x => _random.Next()).ToList();
            _cards = shuffled;
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("Deck is empty!");
            }

            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining => _cards.Count;

        public void Reset(int numberOfDecks = 1)
        {
            InitializeDeck(numberOfDecks);
            Shuffle();
        }
    }
}