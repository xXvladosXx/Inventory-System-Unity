using System;
using InventorySystem.Items;

namespace InventorySystem.UI.Common
{
    [Serializable]
    public abstract class SlotCondition
    {
        public abstract bool IsMet(InventoryItem inventoryItem);
    }
}