using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Types;
using InventorySystem.UI.Panels;
using InventorySystem.UI.Slots;
using InventorySystem.UI.Slots.SlotType;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public abstract class ItemClickAction
    {
        public BaseItemContainerPanel StartPanel { get; }
        public BaseItemContainerPanel EndPanel { get; }
        public ItemContainer StartItemContainer { get; }
        public ItemContainer EndItemContainer { get; }
        public abstract string Name { get; }
        public abstract Type ActionType { get; }
        public ItemClickAction(BaseItemContainerPanel startPanel, BaseItemContainerPanel endPanel,
            ItemContainer startItemContainer, ItemContainer endItemContainer)
        {
            StartPanel = startPanel;
            EndPanel = endPanel;
            StartItemContainer = startItemContainer;
            EndItemContainer = endItemContainer;
        }
        
        public abstract bool OnActionClickSuccess(ItemClickContext context);
    }
}