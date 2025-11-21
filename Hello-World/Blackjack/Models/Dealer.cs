namespace Blackjack.Models
{
    public class Dealer
    {
        public Hand Hand { get; private set; }

        public Dealer()
        {
            Hand = new Hand();
        }

        public bool ShouldHit()
        {
            return Hand.GetValue() < 17;
        }

        public void ResetHand()
        {
            Hand.Clear();
        }

        public void RevealHiddenCard()
        {
            Hand.RevealAll();
        }
    }
}