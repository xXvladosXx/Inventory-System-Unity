using System.Collections.Generic;
using InventorySystem.Items.Properties;
using UnityEngine;
using PropertyName = InventorySystem.Items.Properties.PropertyName;

namespace InventorySystem.Slots
{
    public interface IItem
    {
        public int ID { get; set; }
        string Name { get; set; }
        bool IsStackable { get; }
        public int MaxInStack { get; }
        public Sprite Icon { get; set; }
        public Dictionary<PropertyName, Property> Properties { get; }
    }
}