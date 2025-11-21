namespace BlackjackApp.Models;

public abstract class Person
{
    public Hand Hand { get; } = new();
    public string Name { get; protected set; } = "Unknown";

    public virtual void TakeCard(Card card)
    {
        Hand.AddCard(card);
    }

    public void ResetHand()
    {
        Hand.Clear();
    }
}