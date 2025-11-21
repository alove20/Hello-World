using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack.Models
{
    public class Hand
    {
        private List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public List<Card> GetCards()
        {
            return new List<Card>(cards);
        }

        public int GetValue()
        {
            int total = 0;
            int aceCount = 0;

            foreach (var card in cards)
            {
                total += card.GetValue();
                if (card.IsAce())
                {
                    aceCount++;
                }
            }

            // Adjust for Aces (count as 1 instead of 11 if busting)
            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }

        public bool IsBusted()
        {
            return GetValue() > 21;
        }

        public bool IsBlackjack()
        {
            return cards.Count == 2 && GetValue() == 21;
        }

        public void Clear()
        {
            cards.Clear();
        }

        public int CardCount()
        {
            return cards.Count;
        }

        public string GetValueDisplay()
        {
            int value = GetValue();
            int aceCount = cards.Count(c => c.IsAce());
            
            // Show alternative value if there are aces and current value is different
            int rawTotal = cards.Sum(c => c.GetValue());
            if (aceCount > 0 && rawTotal != value && value <= 21)
            {
                int altValue = rawTotal;
                while (altValue > 21 && aceCount > 0)
                {
                    altValue -= 10;
                    aceCount--;
                }
                if (altValue != value)
                {
                    return $"{value}";
                }
            }
            
            return value.ToString();
        }
    }
}