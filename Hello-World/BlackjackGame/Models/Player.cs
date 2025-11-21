namespace BlackjackGame.Models
{
    public class Player
    {
        public Hand Hand { get; private set; }
        public decimal Balance { get; private set; }
        public decimal CurrentBet { get; set; }

        public Player(decimal startingBalance)
        {
            Hand = new Hand();
            Balance = startingBalance;
            CurrentBet = 0;
        }

        public bool PlaceBet(decimal amount)
        {
            if (amount <= 0)
                return false;

            if (amount > Balance)
                return false;

            CurrentBet = amount;
            Balance -= amount;
            return true;
        }

        public void Win(decimal multiplier = 1.0m)
        {
            Balance += CurrentBet * (1 + multiplier);
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

        public bool HasMoney() => Balance > 0;
    }
}