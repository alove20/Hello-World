using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame.Models
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

        public List<Card> GetCards() => _cards;

        public int GetValue()
        {
            int value = 0;
            int aceCount = 0;

            foreach (var card in _cards)
            {
                if (card.IsFaceDown)
                    continue;

                value += card.GetValue();
                if (card.IsAce())
                    aceCount++;
            }

            // Adjust for Aces: if hand value exceeds 21, convert Aces from 11 to 1
            while (value > 21 && aceCount > 0)
            {
                value -= 10; // Convert one Ace from 11 to 1
                aceCount--;
            }

            return value;
        }

        public bool IsBusted() => GetValue() > 21;

        public bool IsBlackjack() => _cards.Count == 2 && GetValue() == 21;

        public void Clear()
        {
            _cards.Clear();
        }

        public int CardCount => _cards.Count;
    }
}