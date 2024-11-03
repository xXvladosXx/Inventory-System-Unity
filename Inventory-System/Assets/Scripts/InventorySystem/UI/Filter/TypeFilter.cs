using System;
using System.Collections.Generic;
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

        public override List<InventoryItem> Filter(ItemContainer itemContainer)
        {
            return itemContainer.FilterItems(item => item.Item.ItemType == _itemType);
        }
    }
}