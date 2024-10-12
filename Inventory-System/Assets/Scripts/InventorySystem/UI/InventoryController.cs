using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Loot;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem.UI
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private Canvas _inventoryCanvas;
        [SerializeField] private Transform _inventoryParent;
        [SerializeField] private DynamicSlotsPanel _equipmentPanel;
        [SerializeField] private ConstantSlotsPanel _inventoryPanel;
        [SerializeField] private BaseItemContainerPanel _lootPanel;
        
        [SerializeField] private ItemContainer _inventoryContainer;
        [SerializeField] private ItemContainer _equipmentContainer;

        [SerializeField] private LootOpener _lootOpener;
        
        public List<InventoryItem> InitialItems = new List<InventoryItem>();
        
        [SerializeField] private Item _item;
        [SerializeField] private Item _item2;
        
        private ItemContainer _currentLootContainer;

        private List<BaseItemContainerPanel> _containerPanels = new List<BaseItemContainerPanel>();

        private void Start()
        {
            PrepareUI();
            PrepareContainers();

            _lootOpener.OnLootContainerOpened += OpenLootContainer;
        }

        private void PrepareUI()
        {
            CreatePanel(_equipmentPanel, _equipmentPanel.transform.position, _equipmentContainer);
            CreatePanel(_inventoryPanel, _inventoryPanel.transform.position, _inventoryContainer);

            _equipmentPanel.gameObject.SetActive(false);
            _inventoryPanel.gameObject.SetActive(false);
        }

        private void CreatePanel(BaseItemContainerPanel panelToCreate, Vector3 position, ItemContainer itemContainer)
        {
            var createdPanel = Instantiate(panelToCreate, position, Quaternion.identity, _inventoryParent);
            
            createdPanel.Initialize(itemContainer);
            createdPanel.ResetAllItems();
            
            createdPanel.OnStartDrag += OnStartDrag;
            createdPanel.OnSwapRequested += OnSwapRequested;
            createdPanel.OnItemActionRequested += OnItemActionRequested;
            
            _containerPanels.Add(createdPanel);
        }

        private void PrepareContainers()
        {
            _inventoryContainer.Initialize();
            _inventoryContainer.OnItemsUpdated += OnInventoriesUpdated;
            
            _equipmentContainer.Initialize();
            _equipmentContainer.OnItemsUpdated += OnInventoriesUpdated;
            
            foreach (var initialItem in InitialItems)
            {
                if (initialItem.IsEmpty)
                    continue;
                
                _inventoryContainer.AddItem(initialItem);
            }
        }

        private void OnDestroy()
        {
            foreach (var panel in _containerPanels)
            {
                panel.OnStartDrag -= OnStartDrag;
                panel.OnSwapRequested -= OnSwapRequested;
                panel.OnItemActionRequested -= OnItemActionRequested;
            }
            
            _inventoryContainer.OnItemsUpdated -= OnInventoriesUpdated;
            _equipmentContainer.OnItemsUpdated -= OnInventoriesUpdated;
            _lootOpener.OnLootContainerOpened -= OpenLootContainer;
        }

        private void OpenLootContainer(ItemContainer lootContainer)
        {
            if (_currentLootContainer != null)
            {
                _currentLootContainer.OnItemsUpdated -= OnInventoriesUpdated;
            }
            
            _currentLootContainer = lootContainer;
            _currentLootContainer.Initialize();
            _currentLootContainer.OnItemsUpdated += OnInventoriesUpdated;

            foreach (var initialItem in InitialItems)
            {
                if (initialItem.IsEmpty)
                    continue;
                
                _currentLootContainer.AddItem(initialItem);
                break;
            }
            
            _lootPanel.DisposeSlots();
            _lootPanel.Initialize(_currentLootContainer);
            
            _lootPanel.ResetAllItems();

            foreach (var (index, inventoryItem) in lootContainer.GetContainerState())
            {
                _lootPanel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
            }
            
            _lootPanel.Open();
        }

        private void OnStartDrag(BaseItemContainerPanel inventoryPanel, int index)
        {
            var inventoryItem = inventoryPanel.ItemContainer.GetItem(index);
            if (inventoryItem.IsEmpty)
                return;
            
            inventoryPanel.CreateDragItem(inventoryItem.Item.Icon, inventoryItem.Amount, index);
        }

        private void OnSwapRequested(ItemContainer startContainer, ItemContainer endContainer, 
            int index1, int index2)
        {
            if (startContainer == endContainer)
            {
                startContainer.SwapItems(index1, index2);
            }
            else
            {
                var itemFromStartContainer = startContainer.GetItem(index1);
                var itemFromEndContainer = endContainer.GetItem(index2);
                startContainer.SetItem(index1, itemFromEndContainer);
                endContainer.SetItem(index2, itemFromStartContainer);
            }
        }

        private void OnItemActionRequested(int index)
        {
            
        }

        private void OnInventoriesUpdated(Dictionary<int, InventoryItem> inventoryItems,
            ItemContainer itemContainer)
        {
            foreach (var panel in _containerPanels)
            {
                if (panel.ItemContainer == itemContainer)
                {
                    panel.ResetAllItems();

                    foreach (var (index, inventoryItem) in inventoryItems)
                    {
                        panel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
                    }
                }
            }

            if (_lootPanel.ItemContainer == itemContainer)
            {
                _lootPanel.ResetAllItems();
                
                foreach (var (index, inventoryItem) in inventoryItems)
                {
                    _lootPanel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _lootPanel.Close();
            }
            
            if (Input.GetKeyDown(KeyCode.I))
            {

                foreach (var occupiedSlot in _inventoryContainer.GetContainerState())
                {
                    _equipmentPanel.UpdateSlot(occupiedSlot.Key, occupiedSlot.Value.Item.Icon, occupiedSlot.Value.Amount);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                var inventoryItem = new InventoryItem(_item, 1);
                _inventoryContainer.AddItem(inventoryItem);
                
                foreach (var occupiedSlot in _inventoryContainer.GetContainerState())
                {
                    _equipmentPanel.UpdateSlot(occupiedSlot.Key, occupiedSlot.Value.Item.Icon, occupiedSlot.Value.Amount);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                var inventoryItem = new InventoryItem(_item2, 1);
                _inventoryContainer.AddItem(inventoryItem);
                
                foreach (var occupiedSlot in _inventoryContainer.GetContainerState())
                {
                    _equipmentPanel.UpdateSlot(occupiedSlot.Key, occupiedSlot.Value.Item.Icon, occupiedSlot.Value.Amount);
                }            
            }
        }
    }
}