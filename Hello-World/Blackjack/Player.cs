namespace Hello_World.Blackjack;

public sealed class Player
{
    public string Name { get; }
    public Hand Hand { get; } = new();
    public decimal Balance { get; private set; }

    public Player(string name, decimal startingBalance)
    {
        Name = name;
        Balance = startingBalance;
    }

    public bool CanBet(decimal amount) => amount > 0 && amount <= Balance;

    public void ApplyWin(decimal amount)
    {
        Balance += amount;
    }

    public void ApplyLoss(decimal amount)
    {
        Balance -= amount;
        if (Balance < 0) Balance = 0;
    }
}