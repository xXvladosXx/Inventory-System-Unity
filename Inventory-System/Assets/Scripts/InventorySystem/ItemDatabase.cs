using System;
using System.Collections.Generic;
using InventorySystem.Slots;
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

        private int _nextAvailableID = 1;

#if UNITY_EDITOR
        [Button]
        public void FindItemsInProject()
        {
            NameItemsDictionary.Clear();
            string[] guids = AssetDatabase.FindAssets("t:Item");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(path);

                if (item != null)
                {
                    NameItemsDictionary.TryAdd(item.name, item);
                    
                    if (IDsItemsDictionary.ContainsKey(item.ID))
                    {
                        continue;
                    }

                    if (item.ID <= 0)
                    {
                        item.ID = GenerateUniqueID();
                        EditorUtility.SetDirty(item); 
                        Debug.Log($"Assigned new ID {item.ID} to item at path: {path}");
                    }

                    IDsItemsDictionary.Add(item.ID, item);
                    NameItemsDictionary.Add(item.Name, item);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(); 
        }
#endif


        private int GenerateUniqueID()
        {
            while (IDsItemsDictionary.ContainsKey(_nextAvailableID))
            {
                _nextAvailableID++;
            }
            return _nextAvailableID;
        }
        
        public Item FindItemByName(string itemName)
        {
            if (NameItemsDictionary.TryGetValue(itemName, out Item item))
            {
                return item as Item;
            }
           
            return null;
        }
    }

}