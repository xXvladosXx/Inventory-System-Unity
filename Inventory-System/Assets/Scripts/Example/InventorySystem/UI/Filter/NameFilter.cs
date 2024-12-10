using System;
using System.Collections.Generic;
using System.Linq;
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

        public override Dictionary<int, InventoryItem> Filter(Dictionary<int, InventoryItem> items)
        {
            return items
                .Where(pair => pair.Value.Item.Name.ToLower().Contains(_searchTerm))
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}