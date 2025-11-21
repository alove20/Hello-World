using System.Collections.Generic;
using System.Linq;

namespace Blackjack.Models
{
    public class Hand
    {
        private List<Card> _cards;

        public Hand()
        {
            _cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public List<Card> Cards => _cards;

        public int GetValue()
        {
            int total = 0;
            int aceCount = 0;

            foreach (var card in _cards.Where(c => !c.IsFaceDown))
            {
                total += card.GetValue();
                if (card.IsAce)
                {
                    aceCount++;
                }
            }

            // Adjust for Aces
            while (total > 21 && aceCount > 0)
            {
                total -= 10; // Convert Ace from 11 to 1
                aceCount--;
            }

            return total;
        }

        public bool IsBusted => GetValue() > 21;

        public bool IsBlackjack => _cards.Count == 2 && GetValue() == 21;

        public void Clear()
        {
            _cards.Clear();
        }

        public int CardCount => _cards.Count;

        public void RevealAll()
        {
            foreach (var card in _cards)
            {
                card.IsFaceDown = false;
            }
        }
    }
}