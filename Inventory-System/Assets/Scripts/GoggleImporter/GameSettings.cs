using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;
using UnityEditor;
using UnityEngine;
using PropertyName = InventorySystem.Items.Properties.PropertyName;

namespace GoggleImporter
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public ItemDatabase ItemDatabase;
        public List<ItemSettings> Items;
        
#if UNITY_EDITOR
        public void UpdateItems()
        {
            ItemDatabase.FindItemsInProject();
            if (ItemDatabase == null || Items == null)
            {
                Debug.LogError("DatabaseItem or Items is not assigned.");
                return;
            }

            foreach (var itemSettings in Items)
            {
                var item = ItemDatabase.FindItemByName(itemSettings.Name);
                if (item == null)
                {
                    Debug.LogWarning($"Item with name {itemSettings.Name} not found in the project.");
                    continue;
                }

                UpdateItemProperties(item, itemSettings);

                EditorUtility.SetDirty(item);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void UpdateItemProperties(Item item, ItemSettings itemSettings)
        {
            item.Name = itemSettings.Name;
            item.IsStackable = itemSettings.IsStackable;
            item.MaxInStack = itemSettings.MaxInStack;

            item.Properties = new Dictionary<PropertyName, Property>();

            foreach (var property in itemSettings.OneValueProperties)
            {
                if (Enum.TryParse(property.Name, out PropertyName propertyName))
                {
                    item.Properties.Add(propertyName, new OneValueProperty() { Name = property.Name, Value = property.Value });
                }
                else
                {
                    Debug.LogWarning($"Property name '{property.Name}' does not match any PropertyName enum value.");
                }
            }
        }
#endif
    }
}