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

    public class Resource
    {
        public ResourceType Type { get; set; }
        public int Quantity { get; set; }

        public Resource(ResourceType type, int quantity)
        {
            Type = type;
            Quantity = quantity;
        }
    }
}