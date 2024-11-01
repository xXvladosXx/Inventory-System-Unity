using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.UI.Slots;
using InventorySystem.UI.Slots.SlotType;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public abstract class ItemClickAction
    {
        public abstract bool OnActionClick(ItemClickContext context);
        public abstract string Name { get; }
    }
}