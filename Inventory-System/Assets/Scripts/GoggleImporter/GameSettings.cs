using System;
using System.Collections.Generic;
using GoggleImporter.ItemParser;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using InventorySystem;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using PropertyName = InventorySystem.Items.Properties.PropertyName;

namespace GoggleImporter
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings")]
    public class GameSettings : SerializedScriptableObject
    {
        public ItemDatabase ItemDatabase;
        public List<ItemSettings> Items;
        
        private readonly PropertySetter _propertyParserManager = new PropertySetter();

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

            item.Properties ??= new Dictionary<PropertyName, Property>();

            var propertiesToRemove = new List<PropertyName>();

            foreach (var property in item.Properties)
            {
                if (property.Value.ResetableOnImport)
                {
                    propertiesToRemove.Add(property.Key);
                }
            }
           
            foreach (var propertyName in propertiesToRemove)
            {
                item.Properties.Remove(propertyName);
            }

            ItemSettingsHelper.SetProperties(_propertyParserManager, itemSettings, item);
        }
#endif
    }
}