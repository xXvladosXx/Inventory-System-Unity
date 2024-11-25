using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using TMPro;
using UnityEngine;

namespace InventorySystem.UI.Filter
{
    [Serializable]
    public class TypeFilter : ItemFilter
    {
        [SerializeField] private TMP_InputField _searchInputField;
        
        private readonly ItemType _itemType;

        public TypeFilter(ItemType itemType)
        {
            _itemType = itemType;
        }

        public override Dictionary<int, InventoryItem> Filter(Dictionary<int, InventoryItem> items)
        {
            return items
                .Where(pair => pair.Value.Item.ItemType == _itemType)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}