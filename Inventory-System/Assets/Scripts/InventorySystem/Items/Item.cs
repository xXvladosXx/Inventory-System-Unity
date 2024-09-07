using System;
using System.Collections.Generic;
using InventorySystem.Items.Properties;
using Newtonsoft.Json;
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
        
        [JsonIgnore]
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public Dictionary<PropertyName, Property> Properties { get; set; } = new Dictionary<PropertyName, Property>();
        
        public void TryGetProperty<T>(PropertyName propertyName, out T property) where T : class
        {
            if (Properties.TryGetValue(propertyName, out var prop))
            {
                property = prop as T;
                return;
            }

            property = null;
        }
    }
}