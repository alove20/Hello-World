using System;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Models
{
    public class Survivor
    {
        public string Name { get; set; }
        public SurvivorSkill Skill { get; set; }
        public int Health { get; set; } // 0..100
        public int Morale { get; set; } // 0..100
        public TaskType AssignedTask { get; set; }
        public bool IsAlive => Health > 0;

        private static readonly Random rand = new Random();

        public Survivor(string name, SurvivorSkill skill)
        {
            Name = name;
            Skill = skill;
            Health = rand.Next(70, 101);
            Morale = rand.Next(60, 101);
            AssignedTask = TaskType.None;
        }

        public void Rest()
        {
            Health = Math.Min(100, Health + 12);
            Morale = Math.Min(100, Morale + 10);
        }

        public void ChangeHealth(int delta)
        {
            Health = Math.Clamp(Health + delta, 0, 100);
        }

        public void ChangeMorale(int delta)
        {
            Morale = Math.Clamp(Morale + delta, 0, 100);
        }
    }
}