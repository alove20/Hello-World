namespace Blackjack
{
    public class Dealer : Player
    {
        public Dealer(string name) : base(name, 0)
        {
        }

        public bool ShouldHit()
        {
            return Hand.GetValue() <= 16;
        }
    }
}