using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame
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
            int value = 0;
            int aceCount = 0;

            foreach (var card in cards)
            {
                value += card.GetValue();
                if (card.RankName == "A")
                {
                    aceCount++;
                }
            }

            // Adjust for Aces: if value > 21 and we have aces counted as 11, convert them to 1
            while (value > 21 && aceCount > 0)
            {
                value -= 10; // Convert an Ace from 11 to 1
                aceCount--;
            }

            return value;
        }

        public bool IsBlackjack()
        {
            return cards.Count == 2 && GetValue() == 21;
        }

        public bool IsBusted()
        {
            return GetValue() > 21;
        }

        public void Clear()
        {
            cards.Clear();
        }

        public void Display(bool hideFirstCard = false)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (i == 0 && hideFirstCard)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[Hidden] ");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = cards[i].GetSuitColor();
                    Console.Write($"[{cards[i]}] ");
                    Console.ResetColor();
                }
            }
        }

        public int GetVisibleValue(bool hideFirstCard = false)
        {
            if (hideFirstCard && cards.Count > 0)
            {
                int value = 0;
                int aceCount = 0;

                for (int i = 1; i < cards.Count; i++)
                {
                    value += cards[i].GetValue();
                    if (cards[i].RankName == "A")
                    {
                        aceCount++;
                    }
                }

                while (value > 21 && aceCount > 0)
                {
                    value -= 10;
                    aceCount--;
                }

                return value;
            }

            return GetValue();
        }
    }
}