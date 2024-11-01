using System;
using InventorySystem.UI.Slots;
using InventorySystem.UI.Slots.SlotType;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class EquipClickAction : ItemClickAction
    {
        public override bool OnActionClick(ItemClickContext context)
        {
            if (ConditionUtils.HasAppropriateSlot(context.LevelSystem, context.EndContainerPanel, context.Item, out var slot))
            {
                TransferItemBetweenPanels(context, slot);
                return true;
            }

            return false;   
        }

        public override string Name => "Equip";

        private void TransferItemBetweenPanels(ItemClickContext context, ContainerSlot targetSlot)
        {
            var targetIndex = context.EndContainerPanel.GetIndexOfSlot(targetSlot);
            var itemInTargetSlot = context.EndContainerPanel.ItemContainer.GetItem(targetIndex);

            context.EndContainerPanel.ItemContainer.SetItem(targetIndex, context.Item);
            context.StartContainerPanel.ItemContainer.SetItem(context.ItemIndex, itemInTargetSlot);

            context.StartContainerPanel.ItemTooltip.ShowTooltip(itemInTargetSlot);
        }
    }
}