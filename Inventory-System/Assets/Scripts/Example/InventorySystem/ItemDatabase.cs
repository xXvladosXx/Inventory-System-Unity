using System;
using System.Collections.Generic;
using GoggleImporter.Runtime.ItemParser.Item;
using GoggleImporter.Runtime.ItemParser.Property;
using InventorySystem.Items;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Database Item", menuName = "Inventory System/Database Item")]
    public class ItemDatabase : SerializedScriptableObject
    {
        [field: SerializeField] public Dictionary<int, IParsableItem> IDsItemsDictionary { get; private set; } = new Dictionary<int, IParsableItem>();
        [field: SerializeField] public Dictionary<string, IParsableItem> NameItemsDictionary { get; private set; } = new Dictionary<string, IParsableItem>();
        [SerializeField] private int _lastID = 1;

        private int _nextAvailableID = 1;

#if UNITY_EDITOR
        [Button]
        public void FindItemsInProject()
        {
            NameItemsDictionary.Clear();
            IDsItemsDictionary.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Item");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(path);

                if (item != null)
                {
                    if (!NameItemsDictionary.TryAdd(item.name, item))
                    {
                        Debug.LogWarning($"Duplicate item name '{item.name}' found at path {path}. Skipping.");
                        continue;
                    }

                    if (item.ID <= 0 || IDsItemsDictionary.ContainsKey(item.ID))
                    {
                        item.ID = GenerateUniqueID(); 
                        EditorUtility.SetDirty(item);
                    }
                    
                    if (item.ID > _lastID)
                        _lastID = item.ID;

                    IDsItemsDictionary[item.ID] = item;
                    Debug.Log($"Item '{item.name}' assigned ID: {item.ID} (Path: {path})");
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif

        private int GenerateUniqueID()
        {
            while (IDsItemsDictionary.ContainsKey(_lastID))
            {
                _lastID++;
            }
            
            EditorUtility.SetDirty(this);
        
            return _lastID++;
        }
        
        public Item FindItemByName(string itemName)
        {
            if (NameItemsDictionary.TryGetValue(itemName, out IParsableItem item))
            {
                return item as Item;
            }
           
            return null;
        }
        
        public Item CreateScriptableObjectWithName(string itemName) => CreateAssetWithName<Item>(itemName);

        private T CreateAssetWithName<T>(string itemName) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string assetPath = $"Assets/Data/InventorySystem/Items/{itemName}.asset";

            if (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null)
            {
                Debug.LogWarning($"Asset with name '{itemName}' already exists. Please choose a different name.");
                return null;
            }

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log($"ScriptableObject '{itemName}' created at {assetPath}");

            return asset;
        }
    }
}