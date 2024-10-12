using System.Collections.Generic;
using System.Linq;
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
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public Dictionary<ActionType, List<Property>> Properties { get; set; } = new Dictionary<ActionType, List<Property>>();
        
        public bool TryGetProperty<TProperty>(ActionType actionType, out List<TProperty> properties) where TProperty : Property
        {
            properties = null;

            if (Properties.TryGetValue(actionType, out var props))
            {
                properties = props.OfType<TProperty>().ToList();
                return properties.Count > 0;
            }

            return false;
        }
    }
}