using System;
using InventorySystem.Items;
using InventorySystem.UI.Panels;

namespace InventorySystem.UI.ClickAction
{
    [Serializable]
    public class TransferClickAction : UnequipClickAction
    {
        public override string Name => $"To {EndPanel.ContainerName}";

        public TransferClickAction(BaseItemContainerPanel startPanel, BaseItemContainerPanel endPanel,
            ItemContainer startItemContainer, ItemContainer endItemContainer) 
            : base(startPanel, endPanel, startItemContainer, endItemContainer) { }
    }
}