using System;
using System.Collections.Generic;
using GoggleImporter.ItemParser;
using GoggleImporter.ItemParser.PropertySetters;
using GoggleImporter.PropertyParser;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GoggleImporter
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings")]
    public class GameSettings : SerializedScriptableObject
    {
        public ItemDatabase ItemDatabase;
        public List<ItemSettings> Items;
        public List<string> PropertyNames = new List<string>();
        
        private readonly PropertySettersCollector _propertyParserManager = new PropertySettersCollector();

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

            item.Properties ??= new Dictionary<PropertyType, List<Property>>();

            var propertiesToRemove = new List<KeyValuePair<PropertyType, Property>>();

            foreach (var kvp in item.Properties)
            {
                var propertyName = kvp.Key;
                var propertyList = kvp.Value;

                foreach (var property in propertyList)
                {
                    if (property.ResetableOnImport)
                    {
                        propertiesToRemove.Add(new KeyValuePair<PropertyType, Property>(propertyName, property));
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

            _propertyParserManager.SetProperties(itemSettings, item);
        }
        
        public void UpdatePropertyNames()
        {
            EnumGenerator.GenerateEnum(PropertyNames.ToArray());
            
            PropertyNames.Clear();
        }
#endif
    }
}