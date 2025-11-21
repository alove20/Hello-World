namespace Blackjack
{
    public class Player
    {
        public Hand Hand { get; } = new Hand();
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public Player(string name, decimal startingBalance)
        {
            Name = name;
            Balance = startingBalance;
        }

        public void ResetHand()
        {
            Hand.Cards.Clear();
        }
    }
}