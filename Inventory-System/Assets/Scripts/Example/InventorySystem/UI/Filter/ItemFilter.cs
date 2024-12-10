using System;
using System.Collections.Generic;
using InventorySystem.Items;

namespace InventorySystem.UI.Filter
{
    [Serializable]
    public abstract class ItemFilter
    {
        public abstract Dictionary<int, InventoryItem> Filter(Dictionary<int, InventoryItem> items);
    }
}