using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.UI.ClickAction;
using InventorySystem.UI.Panels;
using StatsSystem.Level;

namespace InventorySystem.UI.Slots.SlotType
{
    public static class ConditionUtils
    {
        public static bool HasAppropriateSlot(LevelSystem levelSystem, BaseItemContainerPanel endPanel, InventoryItem item, out ContainerSlot appropriateSlot)
        {
            var appropriateSlots = new List<ContainerSlot>();
            
            foreach (var slot in endPanel.Slots)
            {
                if (IsConditionMet(levelSystem, slot, item))
                {
                    appropriateSlots.Add(slot);
                }
            }

            if (appropriateSlots.Count > 1)
            {
                foreach (var slot in appropriateSlots)
                {
                    if (endPanel.ItemContainer.GetItem(endPanel.GetIndexOfSlot(slot)).IsEmpty)
                    {
                        appropriateSlot = slot;
                        return true;
                    }
                }
            }

            if (appropriateSlots.Count == 1)
            {
                appropriateSlot = appropriateSlots[0];
                return true;
            }

            appropriateSlot = null;
            return false;
        }

        public static bool IsConditionMet(LevelSystem levelSystem, ContainerSlot slot, InventoryItem item)
        {
            if (item.IsEmpty)
                return true;
            
            if (slot.Conditions.Count == 0)
                return true;

            var allConditionsMet = true;
            foreach (var condition in slot.Conditions)
            {
                if (!condition.IsMet(levelSystem, item))
                {
                    allConditionsMet = false;
                }
            }

            return allConditionsMet;
        }
    }
}