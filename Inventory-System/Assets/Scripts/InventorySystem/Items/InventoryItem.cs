using System;
using InventorySystem.Slots;

namespace InventorySystem.Items
{
    [Serializable]
    public struct InventoryItem 
    {
        public int Amount;
        public Item Item;
        public bool IsEmpty => Item == null;
        
        public InventoryItem(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
        
        public InventoryItem ChangeAmount(int amount) => new(Item, amount);
        public static InventoryItem CreateEmpty() => new(null, 0);
    }
}