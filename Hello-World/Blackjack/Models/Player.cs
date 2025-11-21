namespace Blackjack.Models
{
    public class Player
    {
        public Hand Hand { get; private set; }
        public decimal Balance { get; private set; }
        public decimal CurrentBet { get; private set; }

        public Player(decimal startingBalance = 100m)
        {
            Hand = new Hand();
            Balance = startingBalance;
            CurrentBet = 0m;
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

        public void Win(decimal multiplier = 1m)
        {
            Balance += CurrentBet * (1 + multiplier);
            CurrentBet = 0m;
        }

        public void Push()
        {
            Balance += CurrentBet;
            CurrentBet = 0m;
        }

        public void Lose()
        {
            CurrentBet = 0m;
        }

        public void ResetHand()
        {
            Hand.Clear();
        }

        public bool HasMoney => Balance > 0;
    }
}