using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using InventorySystem.UI.Filter;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace InventorySystem
{
    public class ItemContainer : SerializedMonoBehaviour
    {
        [field: SerializeField] public int Size { get; private set; } = 10;
        [SerializeField] private List<InventoryItem> _items = new List<InventoryItem>();

        private ItemFilter _currentFilter;
        public bool IsFilterActive => _currentFilter != null;
        public event Action<ItemContainer> OnItemsUpdated;

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
                    
                    OnItemsUpdated?.Invoke(this);
                    return amount;
                }
            }
            
            amount = AddStackableItem(item, amount);
            OnItemsUpdated?.Invoke(this);
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
                        OnItemsUpdated?.Invoke(this);
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
        
        public int RemoveItem(InventoryItem item, int amount)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Item.ID == item.Item.ID)
                {
                    if (_items[i].Amount > amount)
                    {
                        _items[i] = _items[i].ChangeAmount(_items[i].Amount - amount);
                        OnItemsUpdated?.Invoke(this);
                        return 0;
                    }

                    amount -= _items[i].Amount;
                    _items[i] = InventoryItem.CreateEmpty();

                    if (amount <= 0)
                    {
                        OnItemsUpdated?.Invoke(this);
                        return 0;
                    }
                }
            }
        
            OnItemsUpdated?.Invoke(this);
            return amount; 
        }

        public void RemoveItemAtIndex(int index, int amount)
        {
            if (index < 0 || index >= _items.Count || _items[index].IsEmpty)
                return;

            if (_items[index].Amount > amount)
            {
                _items[index] = _items[index].ChangeAmount(_items[index].Amount - amount);
            }
            else
            {
                _items[index] = InventoryItem.CreateEmpty();
            }

            OnItemsUpdated?.Invoke(this);
        }
        
        public void RemoveItemsAtIndex(int index)
        {
            if (index < 0 || index >= _items.Count || _items[index].IsEmpty)
                return;

            _items[index] = InventoryItem.CreateEmpty();

            OnItemsUpdated?.Invoke(this);
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
            
            if (IsFilterActive)
            {
                return _currentFilter.Filter(occupiedSlots);
            }

            return occupiedSlots;
        }
        
        public void SetFilter(ItemFilter filter)
        {
            _currentFilter = filter;
        }

        public void ResetFilter()
        {
            _currentFilter = null;
        }
        
        public InventoryItem GetItem(int index) => _items[index];

        public void AddItem(InventoryItem item) => AddItem(item.Item, item.Amount);
        
        public List<InventoryItem> FilterItems(Func<InventoryItem, bool> predicate) => 
            _items.Where(item => !item.IsEmpty && predicate(item)).ToList();

        public int IndexOf(InventoryItem targetItem)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Item == targetItem.Item)
                {
                    return i;
                }
            }
            return -1; 
        }

        public int GetFirstEmptySlotIndex()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].IsEmpty)
                {
                    return i;
                }
            }
            return -1; 
        }

        public void SwapItems(int index1, int index2)
        {
            (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
            OnItemsUpdated?.Invoke(this);
        }

        public void SetItem(int index, InventoryItem item)
        {
            _items[index] = item;
            OnItemsUpdated?.Invoke(this);
        }
    }
}
