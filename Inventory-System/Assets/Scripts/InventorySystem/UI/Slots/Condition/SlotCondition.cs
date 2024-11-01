using System;
using InventorySystem.Items;
using StatsSystem.Level;

namespace InventorySystem.UI.Slots.SlotType
{
    [Serializable]
    public abstract class SlotCondition
    {
        public abstract bool IsMet(LevelSystem levelSystem, InventoryItem inventoryItem);
    }
}