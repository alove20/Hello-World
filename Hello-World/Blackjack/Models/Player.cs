namespace Blackjack.Models
{
    public class Player
    {
        public Hand Hand { get; private set; }
        public decimal Balance { get; private set; }
        public decimal CurrentBet { get; private set; }

        public Player(decimal startingBalance)
        {
            Hand = new Hand();
            Balance = startingBalance;
            CurrentBet = 0;
        }

        public bool PlaceBet(decimal amount)
        {
            if (amount <= 0 || amount > Balance)
            {
                return false;
            }

            CurrentBet = amount;
            Balance -= amount;
            return true;
        }

        public void Win(decimal amount)
        {
            Balance += amount;
        }

        public void ClearHand()
        {
            Hand.Clear();
        }

        public void ResetBet()
        {
            CurrentBet = 0;
        }

        public bool HasMoney()
        {
            return Balance > 0;
        }
    }
}