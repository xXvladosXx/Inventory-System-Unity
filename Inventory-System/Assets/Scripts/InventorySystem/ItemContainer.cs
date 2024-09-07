using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Slots;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace InventorySystem
{
    public class ItemContainer : SerializedMonoBehaviour
    {
        [field: SerializeField] public int Size { get; private set; } = 10;
        
        [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();
        [SerializeField] private Item _item;
        [SerializeField] private Item _item2;
        public event Action<Dictionary<int, InventoryItem>> OnItemsUpdated;
        
        public void Initialize()
        {
            for (int i = 0; i < Size; i++)
            {
                _items.Add(InventoryItem.CreateEmpty());
            }
        }

        public int AddItem(Item item, int amount)
        {
            if (item.IsStackable == false)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    while (amount > 0 && IsInventoryFull() == false)
                    {
                        amount -= AddToFirstFreeSlot(item, 1);
                    }
                    
                    OnItemsUpdated?.Invoke(GetContainerState());
                    return amount;
                }
            }
            
            amount = AddStackableItem(item, amount);
            OnItemsUpdated?.Invoke(GetContainerState());
            return amount;
        }

        private bool IsInventoryFull() => _items.Any(item => item.IsEmpty) == false;

        private int AddToFirstFreeSlot(Item item, int amount)
        {
            var newItem = new InventoryItem(item, amount);
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].IsEmpty)
                {
                    _items[i] = newItem;
                    return amount;
                }
            }
            
            return 0;
        }

        private int AddStackableItem(Item item, int amount)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].IsEmpty)
                    continue;
                
                if (_items[i].Item.ID == item.ID)
                {
                    int remainingAmount = item.MaxInStack - _items[i].Amount;
                    if (remainingAmount >= amount)
                    {
                        _items[i] = _items[i].ChangeAmount(_items[i].Amount + amount);
                        OnItemsUpdated?.Invoke(GetContainerState());
                        return 0;
                    }
                    
                    _items[i] = _items[i].ChangeAmount(item.MaxInStack);
                    amount -= remainingAmount;
                }
            }

            while (amount > 0 && IsInventoryFull() == false)
            {
                int newAmount = Mathf.Min(amount, item.MaxInStack);
                amount -= AddToFirstFreeSlot(item, newAmount);
            }

            return amount;
        }

        public Dictionary<int, InventoryItem> GetContainerState()
        {
            Dictionary<int, InventoryItem> occupiedSlots = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items[i].IsEmpty)
                {
                    occupiedSlots.Add(i, _items[i]);
                }
            }

            return occupiedSlots;
        }

        public InventoryItem GetItem(int index) => _items[index];

        public void AddItem(InventoryItem item) => AddItem(item.Item, item.Amount);

        public void SwapItems(int index1, int index2)
        {
            (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
            OnItemsUpdated?.Invoke(GetContainerState());
        }
    }
}
