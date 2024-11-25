using System;
using InventorySystem.Items.Types;
using InventorySystem.UI.Panels;
using InventorySystem.UI.Slots;
using InventorySystem.UI.Slots.SlotType;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class EquipClickAction : ItemClickAction
    {
        public override string Name => "Equip";
        public override Type ActionType => typeof(EquippableAction);

        public EquipClickAction(BaseItemContainerPanel startPanel, BaseItemContainerPanel endPanel,
            ItemContainer startItemContainer, ItemContainer endItemContainer) 
            : base(startPanel, endPanel, startItemContainer, endItemContainer) { }

        public override bool OnActionClickSuccess(ItemClickContext context)
        {
            if (ConditionUtils.HasAppropriateSlot(context.LevelSystem, EndPanel, EndItemContainer, context.Item, out var slot))
            {
                TransferItemBetweenPanels(context, slot);
                return true;
            }

            return false;   
        }

        private void TransferItemBetweenPanels(ItemClickContext context, ContainerSlot targetSlot)
        {
            var targetIndex = EndPanel.GetIndexOfSlot(targetSlot);
            var itemInTargetSlot = EndItemContainer.GetItem(targetIndex);

            EndItemContainer.SetItem(targetIndex, context.Item);
            StartItemContainer.SetItem(context.ItemIndex, itemInTargetSlot);
        }
    }
}