namespace ColonyShipExodus.Models
{
    public enum ResourceType
    {
        Food,
        Water,
        Medicine,
        BuildingMaterials,
        Energy
    }

    public enum SurvivorSkill
    {
        Scavenger,
        Builder,
        Farmer,
        Medic
    }

    public enum TaskType
    {
        None = 0,
        Scavenge = 1,
        Build = 2,
        Farm = 3,
        Rest = 4
    }

    public enum BiomeType
    {
        Plains,
        Forest,
        Ruins,
        Hills,
        Water,
        Rocky,
        CrashSite
    }

    public enum BuildingType
    {
        Shelter,
        Farm,
        WaterPurifier,
        Infirmary,
        None
    }

    public enum BuildingStatus
    {
        UnderConstruction,
        Completed,
        Damaged
    }
}