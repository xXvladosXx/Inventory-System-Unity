using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;
using UnityEngine;
using UnityEngine.Serialization;
using PropertyName = InventorySystem.Items.Properties.PropertyName;

namespace InventorySystem.UI
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPanel _inventoryPanel;
        [SerializeField] private ItemContainer _itemContainer;
        
        public List<InventoryItem> InitialItems = new List<InventoryItem>();
        
        [SerializeField] private Item _item;
        [SerializeField] private Item _item2;

        private void Start()
        {
            PrepareUI();
            PrepareContainer();
        }

        private void PrepareContainer()
        {
            _itemContainer.Initialize();
            _itemContainer.OnItemsUpdated += OnItemsUpdated;

            foreach (var initialItem in InitialItems)
            {
                if (initialItem.IsEmpty)
                    continue;
                
                _itemContainer.AddItem(initialItem);
            }
        }

        private void OnDestroy()
        {
            _itemContainer.OnItemsUpdated -= OnItemsUpdated;
            _inventoryPanel.OnStartDrag -= OnStartDrag;
            _inventoryPanel.OnSwapRequested -= OnSwapRequested;
            _inventoryPanel.OnItemActionRequested -= OnItemActionRequested;
        }

        private void PrepareUI()
        {
            _inventoryPanel.Initialize(_itemContainer.Size);
            _inventoryPanel.OnStartDrag += OnStartDrag;
            _inventoryPanel.OnSwapRequested += OnSwapRequested;
            _inventoryPanel.OnItemActionRequested += OnItemActionRequested;
        }

        private void OnStartDrag(int index)
        {
            var inventoryItem = _itemContainer.GetItem(index);
            if (inventoryItem.IsEmpty)
                return;
            
            _inventoryPanel.CreateDragItem(inventoryItem.Item.Icon, inventoryItem.Amount);

            inventoryItem.Item.TryGetProperty<OneValueProperty>(PropertyName.Damage, out var damage);
            if (damage != null)
            {
                Debug.Log($"Damage: {damage.Value}");
            }
        }

        private void OnSwapRequested(int index1, int index2)
        {
            _itemContainer.SwapItems(index1, index2);
        }

        private void OnItemActionRequested(int index)
        {
            
        }

        private void OnItemsUpdated(Dictionary<int, InventoryItem> inventoryItems)
        {
            _inventoryPanel.ResetAllItems();
            
            foreach (var (index, inventoryItem) in inventoryItems)
            {
                _inventoryPanel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _inventoryPanel.Show();

                foreach (var occupiedSlot in _itemContainer.GetContainerState())
                {
                    _inventoryPanel.UpdateSlot(occupiedSlot.Key, occupiedSlot.Value.Item.Icon, occupiedSlot.Value.Amount);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _inventoryPanel.Hide();
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                var inventoryItem = new InventoryItem(_item, 1);
                _itemContainer.AddItem(inventoryItem);
                
                foreach (var occupiedSlot in _itemContainer.GetContainerState())
                {
                    _inventoryPanel.UpdateSlot(occupiedSlot.Key, occupiedSlot.Value.Item.Icon, occupiedSlot.Value.Amount);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                var inventoryItem = new InventoryItem(_item2, 1);
                _itemContainer.AddItem(inventoryItem);
                
                foreach (var occupiedSlot in _itemContainer.GetContainerState())
                {
                    _inventoryPanel.UpdateSlot(occupiedSlot.Key, occupiedSlot.Value.Item.Icon, occupiedSlot.Value.Amount);
                }            
            }
        }
    }
}