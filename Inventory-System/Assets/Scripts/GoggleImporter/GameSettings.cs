using System;
using System.Collections.Generic;
using GoggleImporter.ItemParser;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using GoggleImporter.PropertyParser;
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
        public List<string> PropertyNames = new List<string>();
        
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

            item.Properties ??= new Dictionary<PropertyName, List<Property>>();

            var propertiesToRemove = new List<KeyValuePair<PropertyName, Property>>();

            foreach (var kvp in item.Properties)
            {
                var propertyName = kvp.Key;
                var propertyList = kvp.Value;

                foreach (var property in propertyList)
                {
                    if (property.ResetableOnImport)
                    {
                        propertiesToRemove.Add(new KeyValuePair<PropertyName, Property>(propertyName, property));
                    }
                }
            }

            foreach (var kvp in propertiesToRemove)
            {
                var propertyName = kvp.Key;
                var property = kvp.Value;

                if (item.Properties.TryGetValue(propertyName, out var propertyList))
                {
                    propertyList.Remove(property);

                    if (propertyList.Count == 0)
                    {
                        item.Properties.Remove(propertyName);
                    }
                }
            }

            ItemSettingsHelper.SetProperties(_propertyParserManager, itemSettings, item);
        }
        
        public void UpdatePropertyNames()
        {
            EnumGenerator.GenerateEnum(PropertyNames.ToArray());
            
            PropertyNames.Clear();
        }
#endif
    }
}