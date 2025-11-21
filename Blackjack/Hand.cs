using System.Collections.Generic;

namespace Blackjack
{
    public class Hand
    {
        public List<Card> Cards { get; } = new List<Card>();

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public int GetValue()
        {
            int value = 0;
            int aceCount = 0;

            foreach (Card card in Cards)
            {
                int cardValue = card.GetValue();
                if (card.Rank == Rank.Ace)
                {
                    aceCount++;
                }
                value += cardValue;
            }

            // Adjust for Aces
            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }
    }
}