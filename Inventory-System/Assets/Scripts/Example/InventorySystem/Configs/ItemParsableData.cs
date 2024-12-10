using System;
using System.Collections.Generic;
using Example.InventorySystem.Items;
using GoggleImporter.Runtime.ItemParser.Item;
using GoggleImporter.Runtime.ItemParser.Types;

namespace Example.InventorySystem.Configs
{
    [Serializable]
    public class ItemParsableData : IItemParsableData
    {
        public string Name { get; set; }
        public bool IsStackable { get; set; }
        public int MaxInStack { get; set; }
        public ItemType ItemType { get; set; }

        public ActionType CurrentType { get; private set; }

        public List<ActionTypeToProperty> AllProperties = new List<ActionTypeToProperty>();

        public void SetCurrentType(ActionType actionType)
        {
            CurrentType = actionType;
        }
    }
}