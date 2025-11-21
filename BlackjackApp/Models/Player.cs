namespace BlackjackApp.Models;

public class Player : Person
{
    public decimal Balance { get; private set; }

    public Player(decimal startingBalance)
    {
        Name = "Player";
        Balance = startingBalance;
    }

    public void AdjustBalance(decimal amount)
    {
        Balance += amount;
    }
}