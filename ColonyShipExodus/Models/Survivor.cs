using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

/// <summary>
/// Represents a single survivor in the colony.
/// </summary>
public class Survivor
{
    public string Name { get; }
    public int Health { get; set; }
    public int Morale { get; set; }
    public TaskType CurrentTask { get; set; }

    public Survivor(string name, int health, int morale)
    {
        Name = name;
        Health = health;
        Morale = morale;
        CurrentTask = TaskType.Idle;
    }

    public void AssignTask(TaskType task)
    {
        CurrentTask = task;
    }
}