using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
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
        [field: SerializeField] public ItemType ItemType { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public Dictionary<ActionType, List<Property>> Properties { get; set; } = new Dictionary<ActionType, List<Property>>();
        
        public bool TryGetProperty<TProperty>(Type actionType, out List<TProperty> properties) where TProperty : Property
        {
            properties = null;

            foreach (var entry in Properties)
            {
                if (entry.Key.GetType() == actionType)
                {
                    properties = entry.Value.OfType<TProperty>().ToList();
                    return properties.Count > 0;
                }
            }

            return false;
        }
        
        public bool TryGetProperty<TProperty>(out TProperty property) where TProperty : Property
        {
            property = null;

            foreach (var properties in Properties.Values)
            {
                foreach (var existedProperty in properties)
                {
                    if (existedProperty.GetType() == typeof(TProperty))
                    {
                        property = (TProperty) existedProperty;
                        return true;
                    }
                }
            }

            return false;
        }

        public string GetPropertiesDescription()
        {
            StringBuilder descriptionBuilder = new StringBuilder();

            foreach (var entry in Properties)
            {
                descriptionBuilder.AppendLine($"{entry.Key}");

                foreach (var property in entry.Value)
                {
                    if (property.ToString() == string.Empty)
                        continue;
                    
                    descriptionBuilder.AppendLine($"{property}");
                }

                descriptionBuilder.AppendLine(); 
            }

            return descriptionBuilder.ToString();
        }
    }
}