namespace BlackjackGame.Models
{
    public class Player
    {
        public Hand Hand { get; }
        public decimal Balance { get; set; }
        public decimal CurrentBet { get; set; }

        public Player(decimal startingBalance)
        {
            Hand = new Hand();
            Balance = startingBalance;
            CurrentBet = 0;
        }

        public bool CanBet(decimal amount)
        {
            return Balance >= amount && amount > 0;
        }

        public void PlaceBet(decimal amount)
        {
            if (!CanBet(amount))
            {
                throw new InvalidOperationException("Insufficient funds or invalid bet amount.");
            }
            
            Balance -= amount;
            CurrentBet = amount;
        }

        public void Win(decimal multiplier = 1.0m)
        {
            Balance += CurrentBet * (1 + multiplier);
            CurrentBet = 0;
        }

        public void Lose()
        {
            CurrentBet = 0;
        }

        public void Push()
        {
            Balance += CurrentBet;
            CurrentBet = 0;
        }

        public void Reset(decimal startingBalance)
        {
            Hand.Clear();
            Balance = startingBalance;
            CurrentBet = 0;
        }
    }
}