using System;
using InventorySystem.Items;
using InventorySystem.Items.Types;
using InventorySystem.UI.Panels;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class UnequipClickAction : ItemClickAction
    {
        public override string Name => "Unequip";
        public override Type ActionType => typeof(EquippableAction);

        public UnequipClickAction(BaseItemContainerPanel startPanel, BaseItemContainerPanel endPanel,
            ItemContainer startItemContainer, ItemContainer endItemContainer) 
            : base(startPanel, endPanel, startItemContainer, endItemContainer) { }

        public override bool OnActionClickSuccess(ItemClickContext context)
        {
            if (TryAddItemToEndContainer(context))
            {
                ClearStartContainerSlot(context);
                return true;
            }

            return false;
        }

        private bool TryAddItemToEndContainer(ItemClickContext context)
        {
            return EndItemContainer.AddItem(context.Item.Item, context.Item.Amount) == 0;
        }

        private void ClearStartContainerSlot(ItemClickContext context)
        {
            StartItemContainer.SetItem(context.ItemIndex, new InventoryItem());
        }
    }
}