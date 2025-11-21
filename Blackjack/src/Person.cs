namespace Blackjack.Src;

public abstract class Person
{
    public Hand Hand { get; } = new();
    public string Name { get; }

    protected Person(string name)
    {
        Name = name;
    }
    
    public void ResetHand()
    {
        Hand.Clear();
    }
}

public class Player : Person
{
    public decimal Balance { get; set; }

    public Player(string name, decimal startingBalance) : base(name)
    {
        Balance = startingBalance;
    }
}

public class Dealer : Person
{
    public Dealer() : base("Dealer") { }
    
    public bool ShouldHit()
    {
        return Hand.CalculateValue() < 17;
    }
}