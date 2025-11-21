using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

public class Survivor
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Name { get; }
    public int Health { get; set; }
    public int Morale { get; set; }
    public SurvivorSkill Skill { get; }
    public SurvivorTask CurrentTask { get; set; }

    public Survivor(string name, SurvivorSkill skill)
    {
        Id = _nextId++;
        Name = name;
        Health = 100;
        Morale = 100;
        Skill = skill;
        CurrentTask = SurvivorTask.Idle;
    }
}