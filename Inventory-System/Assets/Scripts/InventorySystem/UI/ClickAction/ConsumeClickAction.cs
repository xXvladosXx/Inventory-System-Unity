using System;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class ConsumeClickAction : ItemClickAction
    {
        public override bool OnActionClick(ItemClickContext context)
        {
            context.StartContainerPanel.ItemContainer.RemoveItemAtIndex(context.ItemIndex, 1);
            return true;
        }

        public override string Name => "Use";
    }
}