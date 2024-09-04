using System.Collections.Generic;
using InventorySystem.Items.Properties;

namespace InventorySystem.Slots
{
    public interface IItem
    {
        public int ID { get; set; }
        string Name { get; set; }
        bool IsStackable { get; }
        public int MaxInStack { get; }
        public Dictionary<PropertyName, Property> Properties { get; }
    }
}