using ColonyShipExodus.Enums;

namespace ColonyShipExodus.Models;

public class Survivor
{
    public string Name { get; }
    public int Health { get; set; } = 100;
    public int Morale { get; set; } = 70;
    public SurvivorTask CurrentTask { get; set; } = SurvivorTask.Idle;

    public Survivor(string name)
    {
        Name = name;
    }

    public void UpdateStatus()
    {
        // Simple degradation over time if not resting
        if (CurrentTask != SurvivorTask.Resting)
        {
            Health = Math.Max(0, Health - 2);
            Morale = Math.Max(0, Morale - 1);
        }
        else
        {
            // Resting recovers health
            Health = Math.Min(100, Health + 10);
            Morale = Math.Min(100, Morale + 5);
        }
    }
}