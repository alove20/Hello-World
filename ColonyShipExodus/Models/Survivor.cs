using System;

namespace ColonyShipExodus.Models
{
    public class Survivor
    {
        public string Name { get; set; }
        public SurvivorSkill PrimarySkill { get; set; }
        public int Health { get; set; }     // 0-100
        public int Morale { get; set; }     // 0-100

        public bool IsAlive => Health > 0;

        public Survivor(string name, SurvivorSkill skill)
        {
            this.Name = name;
            this.PrimarySkill = skill;
            this.Health = 100;
            this.Morale = 60;
        }

        public void ChangeHealth(int amount)
        {
            Health = Math.Min(100, Math.Max(0, Health + amount));
        }
        public void ChangeMorale(int amount)
        {
            Morale = Math.Min(100, Math.Max(0, Morale + amount));
        }
    }
}