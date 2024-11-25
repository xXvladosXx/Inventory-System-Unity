using System;
using InventorySystem.Items.Types;
using InventorySystem.UI.Panels;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class ConsumeClickAction : ItemClickAction
    {
        public override string Name => "Use";
        public override Type ActionType => typeof(ConsumableAction);

        public ConsumeClickAction(BaseItemContainerPanel startPanel, BaseItemContainerPanel endPanel,
            ItemContainer startItemContainer, ItemContainer endItemContainer) 
            : base(startPanel, endPanel, startItemContainer, endItemContainer) { }
        
        public override bool OnActionClickSuccess(ItemClickContext context)
        {
            StartItemContainer.RemoveItemAtIndex(context.ItemIndex, 1);
            return true;
        }
    }
}