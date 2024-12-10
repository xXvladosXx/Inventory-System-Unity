using System;
using System.Collections.Generic;
using Example.StatsSystem.Level;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;

namespace InventorySystem.UI.Slots.SlotType
{
    [Serializable]
    public class EquipTypeCondition : SlotCondition
    {
        public EquipType EquipType;
        
        public override bool IsMet(LevelSystem levelSystem, InventoryItem inventoryItem)
        {
            if (inventoryItem.Item == null)
                return false;

            if (inventoryItem.Item.TryGetProperty(typeof(EquippableAction),
                    out List<EquippableProperty> equippableProperties))
            {
                foreach (var equippableProperty in equippableProperties)
                {
                    if (equippableProperty.EquipType == EquipType)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}