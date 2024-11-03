using System;
using System.Collections.Generic;
using InventorySystem.Items;

namespace InventorySystem.UI.Filter
{
    [Serializable]
    public abstract class ItemFilter
    {
        public abstract List<InventoryItem> Filter(ItemContainer itemContainer);
    }
}