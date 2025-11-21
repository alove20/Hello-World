using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackGame.Models
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

        public void Clear()
        {
            cards.Clear();
        }

        public int GetValue()
        {
            int value = 0;
            int aceCount = 0;

            foreach (var card in cards)
            {
                if (card.Rank == Rank.Ace)
                {
                    aceCount++;
                    value += 11;
                }
                else
                {
                    value += card.GetValue();
                }
            }

            // Adjust for Aces if needed
            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }

            return value;
        }

        public bool IsBust()
        {
            return GetValue() > 21;
        }

        public bool IsBlackjack()
        {
            return cards.Count == 2 && GetValue() == 21;
        }

        public IReadOnlyList<Card> Cards => cards.AsReadOnly();

        public string GetDisplayString(bool hideFirstCard = false)
        {
            if (cards.Count == 0)
                return "";

            if (hideFirstCard && cards.Count > 0)
            {
                return "[Hidden] " + string.Join(" ", cards.Skip(1).Select(c => c.ToString()));
            }

            return string.Join(" ", cards.Select(c => c.ToString()));
        }

        public void DisplayCards(bool hideFirstCard = false)
        {
            if (cards.Count == 0)
                return;

            if (hideFirstCard && cards.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[??] ");
                Console.ResetColor();
                
                for (int i = 1; i < cards.Count; i++)
                {
                    Console.ForegroundColor = cards[i].GetSuitColor();
                    Console.Write(cards[i].ToString());
                    Console.ResetColor();
                    if (i < cards.Count - 1)
                        Console.Write(" ");
                }
            }
            else
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    Console.ForegroundColor = cards[i].GetSuitColor();
                    Console.Write(cards[i].ToString());
                    Console.ResetColor();
                    if (i < cards.Count - 1)
                        Console.Write(" ");
                }
            }
        }
    }
}