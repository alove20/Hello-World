namespace ColonyShipExodus
{
    public class Survivor
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Morale { get; set; }
        public List<Skill> Skills { get; set; }

        public Survivor(string name, int health, int morale)
        {
            Name = name;
            Health = health;
            Morale = morale;
            Skills = new List<Skill>(); // Not fully implemented
        }
    }

    public enum Skill
    {
        Scavenging,
        Building,
        Farming,
        Medical
    }
}