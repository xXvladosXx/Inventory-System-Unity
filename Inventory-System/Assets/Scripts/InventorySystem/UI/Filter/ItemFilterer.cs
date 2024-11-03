using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.UI.Filter;
using InventorySystem.UI.Panels;
using UnityEngine;

namespace InventorySystem.UI
{
    [Serializable]
    public class ItemFilterer
    {
        private readonly ItemContainer _itemContainer;
        private List<int> _filteredIndices = new List<int>();
        private ItemFilter _currentFilter;
        public bool IsFilterActive => _currentFilter != null;

        public ItemFilterer(ItemContainer itemContainer)
        {
            _itemContainer = itemContainer;
        }

        public List<InventoryItem> ApplyFilter(ItemFilter filter)
        {
            _currentFilter = filter;
            var filteredItems = _currentFilter.Filter(_itemContainer);
            _filteredIndices = filteredItems.Select(_itemContainer.IndexOf).ToList();
            return filteredItems;
        }

        public void ResetFilter()
        {
            _currentFilter = null;
            _filteredIndices.Clear();
        }

        public int GetActualIndex(int filteredIndex)
        {
            if (IsFilterActive)
            {if (filteredIndex >= _filteredIndices.Count)
                return _itemContainer.GetFirstEmptySlotIndex();}
            
            return IsFilterActive && filteredIndex >= 0 && filteredIndex < _filteredIndices.Count
                ? _filteredIndices[filteredIndex]
                : filteredIndex;
        }
    }
}