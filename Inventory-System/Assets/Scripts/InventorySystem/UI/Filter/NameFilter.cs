using System;
using System.Collections.Generic;
using InventorySystem.Items;
using TMPro;
using UnityEngine;

namespace InventorySystem.UI.Filter
{
    [Serializable]
    public class NameFilter : ItemFilter
    {
        [SerializeField] private TMP_Dropdown _itemTypeDropdown;

        private readonly string _searchTerm;

        public NameFilter(string searchTerm)
        {
            _searchTerm = searchTerm.ToLower();
        }

        public override List<InventoryItem> Filter(ItemContainer itemContainer)
        {
            return itemContainer.FilterItems(item => item.Item.Name.ToLower().Contains(_searchTerm));
        }
    }
}