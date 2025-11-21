namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single survivor in the colony.
/// </summary>
public class Survivor
{
    public string Name { get; }
    public int Health { get; set; } = 100;
    public int Morale { get; set; } = 75;
    public SurvivorSkill Skill { get; }
    public TaskType AssignedTask { get; set; } = TaskType.Idle;

    public Survivor(string name, SurvivorSkill skill)
    {
        Name = name;
        Skill = skill;
    }

    public override string ToString()
    {
        return $"{Name} ({Skill}) - Health: {Health}, Morale: {Morale}, Task: {AssignedTask}";
    }
}