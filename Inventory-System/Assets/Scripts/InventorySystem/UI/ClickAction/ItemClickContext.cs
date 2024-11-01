using InventorySystem.Items;
using InventorySystem.UI.Panels;
using StatsSystem.Level;

namespace InventorySystem.UI.ClickAction
{
    public class ItemClickContext
    {
        public BaseItemContainerPanel StartContainerPanel { get; }
        public BaseItemContainerPanel EndContainerPanel { get; }
        public LevelSystem LevelSystem { get; }
        public InventoryItem Item { get; }
        public int ItemIndex { get; }

        public ItemClickContext(BaseItemContainerPanel startPanel, 
            BaseItemContainerPanel endPanel,
            LevelSystem levelSystem,
            InventoryItem item, int index)
        {
            StartContainerPanel = startPanel;
            EndContainerPanel = endPanel;
            LevelSystem = levelSystem;
            Item = item;
            ItemIndex = index;
        }
    }
}