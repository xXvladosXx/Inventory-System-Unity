using System;
using Example.StatsSystem.Level;
using InventorySystem.Items;

namespace InventorySystem.UI.Slots.SlotType
{
    [Serializable]
    public abstract class SlotCondition
    {
        public abstract bool IsMet(LevelSystem levelSystem, InventoryItem inventoryItem);
    }
}