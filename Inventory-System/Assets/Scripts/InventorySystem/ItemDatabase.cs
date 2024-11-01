using System;
using System.Collections.Generic;
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
        [field: SerializeField] public Dictionary<int, Item> IDsItemsDictionary { get; private set; } = new Dictionary<int, Item>();
        [field: SerializeField] public Dictionary<string, Item> NameItemsDictionary { get; private set; } = new Dictionary<string, Item>();
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
                    NameItemsDictionary.TryAdd(item.name, item);
                
                    if (IDsItemsDictionary.ContainsKey(item.ID) || item.ID <= 0)
                    {
                        Debug.LogWarning($"Duplicate ID {item.ID} found for item at path: {path}. Generating new ID.");
                        item.ID = GenerateUniqueID();
                        EditorUtility.SetDirty(item); 
                    }
                    
                    if (item.ID > _lastID)
                        _lastID = item.ID;

                    IDsItemsDictionary[item.ID] = item;
                    NameItemsDictionary[item.name] = item;

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
            if (NameItemsDictionary.TryGetValue(itemName, out Item item))
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