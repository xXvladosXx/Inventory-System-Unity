﻿using Example.StatsSystem.Level;
using InventorySystem.Items;
using InventorySystem.UI.Panels;

namespace InventorySystem.UI.ClickAction
{
    public class ItemClickContext
    {
        public LevelSystem LevelSystem { get; }
        public InventoryItem Item { get; }
        public int ItemIndex { get; }

        public ItemClickContext(LevelSystem levelSystem,
            InventoryItem item, int index)
        {
            LevelSystem = levelSystem;
            Item = item;
            ItemIndex = index;
        }
    }
}