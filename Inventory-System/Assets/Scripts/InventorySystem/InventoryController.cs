using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Stats;
using InventorySystem.Items.Types;
using InventorySystem.Loot;
using InventorySystem.UI.ClickAction;
using InventorySystem.UI.ContextMenu;
using InventorySystem.UI.Panels;
using InventorySystem.UI.Slots.SlotType;
using StatsSystem;
using StatsSystem.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem.UI
{
    public class InventoryController : MonoBehaviour, IStatsChangeable
    {
        [SerializeField] private Canvas _inventoryCanvas;
        [SerializeField] private Transform _inventoryParent;
        [SerializeField] private ConstantSlotsPanel _equipmentPanel;
        [SerializeField] private DynamicSlotsPanel _inventoryPanel;
        [SerializeField] private BaseItemContainerPanel _lootPanel;
        [SerializeField] private ItemTooltip _itemTooltip;
        [SerializeField] private ItemContextMenu _itemContextMenu;
        [SerializeField] private LevelSystem _levelSystem;
        
        [SerializeField] private ItemContainer _inventoryContainer;
        [SerializeField] private ItemContainer _equipmentContainer;

        [SerializeField] private LootOpener _lootOpener;
        
        public List<InventoryItem> InitialItems = new List<InventoryItem>();
        
        private ItemContainer _currentLootContainer;

        private readonly List<BaseItemContainerPanel> _containerPanels = new List<BaseItemContainerPanel>();
        private readonly Dictionary<BaseItemContainerPanel, BaseItemContainerPanel> _linkedPanels = new Dictionary<BaseItemContainerPanel, BaseItemContainerPanel>();

        public event Action OnStatsChanged;

        private void Start()
        {
            PrepareUI();
            PrepareContainers();

            _lootOpener.OnLootContainerOpened += OpenLootContainer;
        }

        private void PrepareUI()
        {
            InitializePanel(_equipmentPanel, _equipmentContainer);
            InitializePanel(_inventoryPanel, _inventoryContainer);

            _linkedPanels[_equipmentPanel] = _inventoryPanel;
            _linkedPanels[_inventoryPanel] = _equipmentPanel;
        }

        private void InitializePanel(BaseItemContainerPanel panelToInitialize, ItemContainer itemContainer)
        {
            panelToInitialize.Initialize(itemContainer, _itemTooltip);
            panelToInitialize.ResetAllItems();
            
            panelToInitialize.OnStartDrag += OnStartDrag;
            panelToInitialize.OnDoubleClicked += OnDoubleClicked;
            panelToInitialize.OnSwapRequested += OnSwapRequested;
            panelToInitialize.OnItemActionRequested += OnItemActionRequested;
            
            _containerPanels.Add(panelToInitialize);
        }

        private void CreatePanel(BaseItemContainerPanel panelToCreate, Vector3 position, ItemContainer itemContainer)
        {
            var createdPanel = Instantiate(panelToCreate, position, Quaternion.identity, _inventoryParent);
            
            createdPanel.Initialize(itemContainer, _itemTooltip);
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

        public Dictionary<StatType, List<CoreStat>> CollectStats(Dictionary<StatType, List<CoreStat>> stats)
        { 
            foreach (var inventoryItem in _equipmentContainer.GetContainerState().Values)
            {
                if (inventoryItem.Item.TryGetProperty(typeof(EquippableAction), out List<ConstantStatProperty> statProperties))
                {
                    foreach (var stat in statProperties)
                    {
                        var coreStat = new CoreStat(stat.StatType, stat.Value);

                        if (stats.ContainsKey(stat.StatType))
                        {
                            stats[stat.StatType] ??= new List<CoreStat>();
                            stats[stat.StatType].Add(coreStat);
                        }
                        else
                        {
                            stats.Add(stat.StatType, new List<CoreStat>());
                            stats[stat.StatType].Add(coreStat);
                        }
                    }
                }                
            }

            return stats;
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
            _lootPanel.Initialize(_currentLootContainer, _itemTooltip);
            
            _lootPanel.ResetAllItems();

            foreach (var (index, inventoryItem) in lootContainer.GetContainerState())
            {
                _lootPanel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
            }
            
            _lootPanel.Open();
        }

        private void OnStartDrag(BaseItemContainerPanel inventoryPanel, int index)
        {
            _itemContextMenu.Hide();
            var inventoryItem = inventoryPanel.ItemContainer.GetItem(index);
            if (inventoryItem.IsEmpty)
                return;
            
            inventoryPanel.CreateDragItem(inventoryItem.Item.Icon, inventoryItem.Amount, index);
        }

        private void OnDoubleClicked(BaseItemContainerPanel inventoryPanel, int index)
        {
            _itemContextMenu.Hide();
            var inventoryItem = inventoryPanel.ItemContainer.GetItem(index);
            if (inventoryItem.IsEmpty) 
                return;

            if (_linkedPanels.TryGetValue(inventoryPanel, out var panelAction))
            {
                var context = new ItemClickContext(inventoryPanel, panelAction, _levelSystem, inventoryItem, index);
                foreach (var action in inventoryPanel.ItemContexts.Keys)
                {
                    if (inventoryItem.Item.TryGetProperty<Property>(action.GetType(), out _))
                    {
                        if (inventoryPanel.ItemContexts[action].OnActionClick(context))
                        {
                            OnStatsChanged?.Invoke();
                        }

                        break;
                    }
                }
            }
        }

        private void OnItemActionRequested(BaseItemContainerPanel inventoryPanel, int index)
        {
            var slotPosition = inventoryPanel.Slots[index].transform.position;
            var inventoryItem = inventoryPanel.ItemContainer.GetItem(index);

            var actions = new List<ItemClickAction>();
            
            foreach (var action in inventoryPanel.ItemContexts.Keys)
            {
                if (inventoryItem.Item.TryGetProperty<Property>(action.GetType(), out _))
                {
                    actions.Add(inventoryPanel.ItemContexts[action]);
                }
            }
            
            _itemContextMenu.Show(actions, slotPosition);

            if (_linkedPanels.TryGetValue(inventoryPanel, out var panelAction))
            {
                var context = new ItemClickContext(inventoryPanel, panelAction, _levelSystem, inventoryItem, index);
                
                foreach (var activeOption in _itemContextMenu.ActiveOptions)
                {
                    activeOption.AssignAction(context);
                    activeOption.OnOptionClicked += OnOptionClicked;
                }
            }
        }

        private void OnOptionClicked(ItemClickAction action, ItemClickContext context)
        {
            foreach (var activeOption in _itemContextMenu.ActiveOptions)
            {
                activeOption.OnOptionClicked -= OnOptionClicked;
            }
            
            action.OnActionClick(context);
            OnStatsChanged?.Invoke();
        }

        private void OnSwapRequested(BaseItemContainerPanel startContainer,
            BaseItemContainerPanel endContainer, 
            int index1, int index2)
        {
            if (!ConditionUtils.IsConditionMet(_levelSystem, endContainer.Slots[index2], startContainer.ItemContainer.GetItem(index1))
                || !ConditionUtils.IsConditionMet(_levelSystem, startContainer.Slots[index1], endContainer.ItemContainer.GetItem(index2)))
            {
                _itemTooltip.HideTooltip();
                return;
            }

            if (startContainer == endContainer)
            {
                startContainer.ItemContainer.SwapItems(index1, index2);
            }
            else
            {
                var itemFromStartContainer = startContainer.ItemContainer.GetItem(index1);
                var itemFromEndContainer = endContainer.ItemContainer.GetItem(index2);
                startContainer.ItemContainer.SetItem(index1, itemFromEndContainer);
                endContainer.ItemContainer.SetItem(index2, itemFromStartContainer);
            }
            
            if (startContainer.ItemContainer == _equipmentContainer || endContainer.ItemContainer == _equipmentContainer)
                OnStatsChanged?.Invoke();
        }

        private void OnInventoriesUpdated(Dictionary<int, InventoryItem> inventoryItems,
            ItemContainer itemContainer)
        {
            _itemContextMenu.Hide();

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
    }
}