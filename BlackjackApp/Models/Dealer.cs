namespace BlackjackApp.Models;

public class Dealer : Person
{
    public Dealer()
    {
        Name = "Dealer";
    }

    public bool ShouldHit()
    {
        // Dealer hits on 16 or less, stands on 17 or more.
        return Hand.CalculateScore() < 17;
    }
}