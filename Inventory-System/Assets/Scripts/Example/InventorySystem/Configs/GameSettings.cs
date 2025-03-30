using System.Collections.Generic;
using GoggleImporter.Runtime.ItemParser.Parsers;
using GoggleImporter.Runtime.ItemParser.Property;
using GoggleImporter.Runtime.ItemParser.Types;
using InventorySystem;
using InventorySystem.Items;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Example.InventorySystem.Configs
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings")]
    public class GameSettings : SerializedScriptableObject
    {
        public ItemDatabase ItemDatabase;
        public List<ItemParsableData> Items;
        
        private ItemDataParser<ItemParsableData> _itemParser;

#if UNITY_EDITOR
        public void UpdateItems(ItemDataParser<ItemParsableData> itemsParser)
        {
            _itemParser = itemsParser;
            
            ItemDatabase.FindItemsInProject();
            if (ItemDatabase == null || Items == null)
            {
                Debug.LogError("DatabaseItem or Items is not assigned.");
                return;
            }

            foreach (var itemSettings in Items)
            {
                var item = ItemDatabase.FindItemById(itemSettings.Id);
                if (item == null)
                {
                    Debug.LogWarning($"Item with name {itemSettings.Name} not found in the project. Creating new item.");
                    item = ItemDatabase.CreateScriptableObjectWithName(itemSettings.Name);
                }
                
                UpdateItemProperties(item, itemSettings);

                EditorUtility.SetDirty(item);
            }
            
            ItemDatabase.FindItemsInProject();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void UpdateItemProperties(Item item, ItemParsableData itemParsableSettings)
        {
            item.ID = itemParsableSettings.Id;
            item.Name = itemParsableSettings.Name;
            item.IsStackable = itemParsableSettings.IsStackable;
            item.MaxInStack = itemParsableSettings.MaxInStack;
            item.ItemType = itemParsableSettings.ItemType;
            item.Properties ??= new Dictionary<ActionType, List<Property>>();

            var propertiesToRemove = new List<KeyValuePair<ActionType, Property>>();

            foreach (var kvp in item.Properties)
            {
                var propertyName = kvp.Key;
                var propertyList = kvp.Value;

                foreach (var property in propertyList)
                {
                    if (property.ResetableOnImport)
                    {
                        propertiesToRemove.Add(new KeyValuePair<ActionType, Property>(propertyName, property));
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

            _itemParser.SetProperties(itemParsableSettings, item);
        }
#endif
    }
}