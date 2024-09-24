using System.Collections.Generic;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem.Items
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
    public class Item : SerializedScriptableObject
    {
        [field: SerializeField, ReadOnly] public int ID { get; set; }
        [field: SerializeField, ReadOnly] public string Name { get; set; }
        [field: SerializeField] public bool IsStackable { get; set; }
        [field: SerializeField] public int MaxInStack { get; set; }
        
        [JsonIgnore]
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public Dictionary<PropertyType, List<Property>> Properties { get; set; } = new Dictionary<PropertyType, List<Property>>();
        
        public bool TryGetProperty<T>(PropertyType propertyType, out T property) where T : class
        {
            if (Properties.TryGetValue(propertyType, out var prop))
            {
                property = prop as T;
                return true;
            }

            property = null;
            return false;
        }
    }
}