using System;

namespace BlackjackGame
{
    public class Player
    {
        public decimal Balance { get; private set; }
        public Hand Hand { get; private set; }
        public decimal CurrentBet { get; private set; }

        public Player(decimal startingBalance)
        {
            Balance = startingBalance;
            Hand = new Hand();
            CurrentBet = 0;
        }

        public bool PlaceBet(decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            if (amount > Balance)
            {
                return false;
            }

            CurrentBet = amount;
            Balance -= amount;
            return true;
        }

        public void Win(decimal multiplier = 1.0m)
        {
            decimal winnings = CurrentBet * (1 + multiplier);
            Balance += winnings;
            CurrentBet = 0;
        }

        public void Push()
        {
            Balance += CurrentBet;
            CurrentBet = 0;
        }

        public void Lose()
        {
            CurrentBet = 0;
        }

        public void ResetHand()
        {
            Hand.Clear();
        }

        public bool HasMoney()
        {
            return Balance > 0;
        }
    }
}