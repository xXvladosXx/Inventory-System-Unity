using System;
using System.Collections.Generic;
using InventorySystem.Items.Properties;
using Sirenix.OdinInspector;
using UnityEngine;
using PropertyName = InventorySystem.Items.Properties.PropertyName;

namespace InventorySystem.Slots
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
    public class Item : SerializedScriptableObject, IItem
    {
        [field: SerializeField, ReadOnly] public int ID { get; set; }
        [field: SerializeField, ReadOnly] public string Name { get; set; }
        [field: SerializeField] public bool IsStackable { get; set; }
        [field: SerializeField] public int MaxInStack { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public Dictionary<PropertyName, Property> Properties { get; set; } = new Dictionary<PropertyName, Property>();
        
        public void HasProperty(PropertyName propertyName, out bool hasProperty)
        {
            hasProperty = Properties.ContainsKey(propertyName);
        }
    }
}