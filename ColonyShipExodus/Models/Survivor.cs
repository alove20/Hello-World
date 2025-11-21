namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single survivor in the colony.
/// </summary>
public class Survivor
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Morale { get; set; }
    public SurvivorSkill Skill { get; set; }
    public SurvivorTask CurrentTask { get; set; }

    public Survivor(string name, SurvivorSkill skill)
    {
        Name = name;
        Skill = skill;
        Health = 100;
        Morale = 80;
        CurrentTask = SurvivorTask.Idle;
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"  - {Name,-25} | Health: {Health,3}/100 | Skill: {Skill,-12} | Task: {CurrentTask}");
    }
}