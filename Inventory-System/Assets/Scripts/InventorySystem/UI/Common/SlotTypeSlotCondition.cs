using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem.UI.Common
{
    [Serializable]
    public class SlotTypeSlotCondition : SlotCondition
    {
        public EquipType EquipType;
        
        public override bool IsMet(InventoryItem inventoryItem)
        {
            if (inventoryItem.Item == null)
                return false;

            if (inventoryItem.Item.TryGetProperty(PropertyType.Equippable,
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