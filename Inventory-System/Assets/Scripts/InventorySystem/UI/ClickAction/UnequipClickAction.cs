using System;
using InventorySystem.Items;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class UnequipClickAction : ItemClickAction
    {
        public override bool OnActionClick(ItemClickContext context)
        {
            if (TryAddItemToEndContainer(context))
            {
                ClearStartContainerSlot(context);
                return true;
            }

            return false;
        }

        public override string Name => "Unequip";

        private bool TryAddItemToEndContainer(ItemClickContext context)
        {
            return context.EndContainerPanel.ItemContainer.AddItem(context.Item.Item, context.Item.Amount) == 0;
        }

        private void ClearStartContainerSlot(ItemClickContext context)
        {
            context.StartContainerPanel.ItemContainer.SetItem(context.ItemIndex, new InventoryItem());
            context.StartContainerPanel.ItemTooltip.HideTooltip();
        }
    }
}