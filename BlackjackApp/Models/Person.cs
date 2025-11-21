namespace BlackjackApp.Models;

public abstract class Person
{
    public string Name { get; }
    public Hand Hand { get; } = new();

    protected Person(string name)
    {
        Name = name;
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
}