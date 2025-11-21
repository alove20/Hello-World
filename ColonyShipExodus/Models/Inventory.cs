using System.Collections.Generic;
using ColonyShipExodus.Models;

namespace ColonyShipExodus.Models
{
    public class Inventory
    {
        private Dictionary<ResourceType, int> _items = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Food, 40 },
            { ResourceType.Water, 40 },
            { ResourceType.Medicine, 10 },
            { ResourceType.BuildingMaterials, 20 },
            { ResourceType.Energy, 10 }
        };

        public int this[ResourceType type]
        {
            get => _items.ContainsKey(type) ? _items[type] : 0;
            set => _items[type] = value;
        }

        public void Add(ResourceType type, int amount)
        {
            if (!_items.ContainsKey(type))
                _items[type] = 0;
            _items[type] += amount;
        }
        public bool HasEnough(ResourceType type, int amt) => this[type] >= amt;
    }
}