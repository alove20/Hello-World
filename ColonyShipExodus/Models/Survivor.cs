namespace ColonyShipExodus.Models
{
    public enum Skill
    {
        Scavenging,
        Building,
        Farming,
        Medical,
        None
    }

    public class Survivor
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Morale { get; set; }
        public Skill Skill { get; set; }
        public bool IsAssigned { get; set; }

        public Survivor(string name)
        {
            Name = name;
            Health = 100;
            Morale = 100;
            Skill = Skill.None;
            IsAssigned = false;
        }
    }
}