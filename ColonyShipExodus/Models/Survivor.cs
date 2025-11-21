namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single survivor in the colony.
/// </summary>
public class Survivor
{
    public string Name { get; }
    public int Health { get; private set; }
    public int Morale { get; private set; }
    public SurvivorSkill Skill { get; }
    public SurvivorTask CurrentTask { get; set; }

    public Survivor(string name, SurvivorSkill skill)
    {
        Name = name;
        Skill = skill;
        Health = 100;
        Morale = 100;
        CurrentTask = SurvivorTask.Rest;
    }

    public void TakeDamage(int amount, string reason)
    {
        Health -= amount;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{Name} took {amount} damage from {reason}.");
        Console.ResetColor();
        if (Health <= 0)
        {
            Console.WriteLine($"{Name} has perished.");
        }
    }

    public void ChangeMorale(int amount)
    {
        Morale = Math.Clamp(Morale + amount, 0, 100);
    }
    
    public void Rest()
    {
        Health = Math.Min(100, Health + 10);
        Morale = Math.Min(100, Morale + 10);
    }

    public override string ToString()
    {
        return $"{Name} (Health: {Health}/100, Morale: {Morale}/100, Skill: {Skill}, Task: {CurrentTask})";
    }
}