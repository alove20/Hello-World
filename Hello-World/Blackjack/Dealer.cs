namespace Hello_World.Blackjack;

public sealed class Dealer
{
    public Hand Hand { get; } = new();

    public bool ShouldHit()
    {
        return Hand.GetBestValue() <= 16;
    }
}