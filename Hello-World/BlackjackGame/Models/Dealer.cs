namespace BlackjackGame.Models
{
    public class Dealer
    {
        public Hand Hand { get; }

        public Dealer()
        {
            Hand = new Hand();
        }

        public bool ShouldHit()
        {
            return Hand.GetValue() < 17;
        }
    }
}