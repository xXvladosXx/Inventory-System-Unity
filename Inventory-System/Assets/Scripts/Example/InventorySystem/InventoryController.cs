using System;
using System.Collections.Generic;
using System.Text;
using Example.InventorySystem.Items;
using Example.StatsSystem.Core;
using Example.StatsSystem.Level;
using Example.StatsSystem.Stats;
using GoggleImporter.Runtime.ItemParser.Property;
using GoggleImporter.Runtime.ItemParser.Types;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using InventorySystem.Loot;
using InventorySystem.UI.ClickAction;
using InventorySystem.UI.ContextMenu;
using InventorySystem.UI.Filter;
using InventorySystem.UI.Panels;
using InventorySystem.UI.Slots.SlotType;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventoryController : MonoBehaviour, IStatsChangeable
    {
        [Title("UI Components")]
        [BoxGroup("UI Panels")]
        [SerializeField] private Canvas _inventoryCanvas;
        [BoxGroup("UI Panels")]
        [SerializeField] private Transform _inventoryParent;
        [BoxGroup("UI Panels")]
        [SerializeField] private ConstantSlotsPanel _equipmentPanel;
        [BoxGroup("UI Panels")]
        [SerializeField] private DynamicSlotsPanel _inventoryPanel;
        [BoxGroup("UI Panels")]
        [SerializeField] private BaseItemContainerPanel _lootPanel;
    
        [BoxGroup("Tooltips and Context Menus")]
        [SerializeField] private ItemTooltip _itemTooltip;
        [BoxGroup("Tooltips and Context Menus")]
        [SerializeField] private ItemContextMenu _itemContextMenu;

        [Title("Player Info")]
        [BoxGroup("Player Components")]
        [SerializeField] private LevelSystem _levelSystem;
        [BoxGroup("Player Components")]
        [SerializeField] private PlayerPreviewUI _playerPreviewUI;
        [BoxGroup("Player Components")]
        [SerializeField] private EquipmentPreview _playerModel;

        [Title("Containers")]
        [BoxGroup("Item Containers")]
        [SerializeField] private ItemContainer _inventoryContainer;
        [BoxGroup("Item Containers")]
        [SerializeField] private ItemContainer _equipmentContainer;
        [BoxGroup("Item Containers")]
        [SerializeField] private ItemContainer _firstLootContainer;
        [BoxGroup("Item Containers")]
        [SerializeField] private ItemContainer _secondLootContainer;

        [Title("Loot Settings")]
        [BoxGroup("Loot System")]
        [SerializeField] private LootOpener _lootOpener;
        [BoxGroup("Loot System")]
        [SerializeField] private Button _firstLootOpener;
        [BoxGroup("Loot System")]
        [SerializeField] private Button _secondLootOpener;

        [Title("Initial Setup")]
        [SerializeField, ListDrawerSettings(Expanded = true)]
        public List<InventoryItem> InitialItems = new List<InventoryItem>();
        
        private readonly List<BaseItemContainerPanel> _containerPanels = new List<BaseItemContainerPanel>();
        private readonly List<BaseItemContainerPanel> _openedPanels = new List<BaseItemContainerPanel>();
        private readonly Dictionary<BaseItemContainerPanel, ItemContainer> _panelsToContainers = new Dictionary<BaseItemContainerPanel, ItemContainer>();

        private readonly List<ItemClickAction> _actions = new List<ItemClickAction>();
        public event Action OnStatsChanged;
        
        private void Start()
        {
            PrepareUI();
            PrepareContainers();
            
            _playerModel.DeactivateModels();
            _playerPreviewUI.Initialize(_playerModel);

            _firstLootOpener.onClick.AddListener(() => OpenLootContainer(_firstLootContainer));
            _secondLootOpener.onClick.AddListener(() => OpenLootContainer(_secondLootContainer));
            
            _lootOpener.OnLootContainerOpened += OpenLootContainer;
        }
        
        private void OnDestroy()
        {
            foreach (var panelToInitialize in _containerPanels)
            {
                panelToInitialize.OnStartDrag -= OnStartDrag;
                panelToInitialize.OnClicked -= OnClicked;
                panelToInitialize.OnDoubleClicked -= OnDoubleClicked;
                panelToInitialize.OnSwapRequested -= OnSwapRequested;
                panelToInitialize.OnItemActionRequested -= OnItemActionRequested;
                panelToInitialize.OnItemSearchRequested -= ApplyItemSearch;
                panelToInitialize.OnItemTypeRequested -= ApplyTypeFilter;
                panelToInitialize.OnShowTooltipRequested -= ShowTooltip;
                panelToInitialize.OnHideTooltipRequested -= HideTooltip;
            }
            
            _firstLootOpener.onClick.RemoveAllListeners();
            _secondLootOpener.onClick.RemoveAllListeners();
            
            _inventoryContainer.OnItemsUpdated -= OnInventoriesUpdated;
            _equipmentContainer.OnItemsUpdated -= OnInventoriesUpdated;
            _lootOpener.OnLootContainerOpened -= OpenLootContainer;
        }

        private void PrepareUI()
        {
            InitializePanel(_equipmentPanel, _equipmentContainer);
            InitializePanel(_inventoryPanel, _inventoryContainer);

            _actions.Add(new EquipClickAction(_inventoryPanel, _equipmentPanel, _inventoryContainer, _equipmentContainer));
            _actions.Add(new ConsumeClickAction(_inventoryPanel, _equipmentPanel, _inventoryContainer, _equipmentContainer));
            _actions.Add(new UnequipClickAction(_equipmentPanel, _inventoryPanel, _equipmentContainer, _inventoryContainer));
        }

        private void InitializePanel(BaseItemContainerPanel panelToInitialize, ItemContainer itemContainer)
        {
            panelToInitialize.Initialize(itemContainer.Size);
            panelToInitialize.ResetAllItems();
            
            panelToInitialize.OnStartDrag += OnStartDrag;
            panelToInitialize.OnClicked += OnClicked;
            panelToInitialize.OnDoubleClicked += OnDoubleClicked;
            panelToInitialize.OnSwapRequested += OnSwapRequested;
            panelToInitialize.OnItemActionRequested += OnItemActionRequested;
            panelToInitialize.OnItemSearchRequested += ApplyItemSearch;
            panelToInitialize.OnItemTypeRequested += ApplyTypeFilter;
            panelToInitialize.OnShowTooltipRequested += ShowTooltip;
            panelToInitialize.OnHideTooltipRequested += HideTooltip;

            _panelsToContainers.Add(panelToInitialize, itemContainer);
            _containerPanels.Add(panelToInitialize);
        }

        private BaseItemContainerPanel CreatePanel(BaseItemContainerPanel panelToCreate)
        {
            var parent = panelToCreate.transform.parent;
            var containerPanel = Instantiate(panelToCreate, parent);
            containerPanel.transform.SetAsLastSibling();

            containerPanel.gameObject.SetActive(true);
            
            return containerPanel;
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
            foreach (var openedPanel in _openedPanels)
            {
                if (_panelsToContainers[openedPanel] == lootContainer)
                {
                    ClosePanel(openedPanel);
                    return;
                }
            }
            
            var createdPanel = CreatePanel(_lootPanel);
            InitializePanel(createdPanel, lootContainer);
            
            _openedPanels.Add(createdPanel);
            
            _actions.Add(new EquipClickAction(createdPanel, _equipmentPanel, lootContainer, _equipmentContainer));
            _actions.Add(new ConsumeClickAction(createdPanel, createdPanel, lootContainer, lootContainer));
            _actions.Add(new TransferClickAction(createdPanel, _inventoryPanel, lootContainer, _inventoryContainer));
            _actions.Add(new TransferClickAction(_equipmentPanel, createdPanel, _equipmentContainer, lootContainer));
            _actions.Add(new TransferClickAction(_inventoryPanel, createdPanel, _inventoryContainer, lootContainer));
            
            lootContainer.Initialize();
            lootContainer.OnItemsUpdated += OnInventoriesUpdated;

            createdPanel.DisposeSlots();
            createdPanel.Initialize(lootContainer.Size);
            
            createdPanel.ResetAllItems();

            foreach (var (index, inventoryItem) in lootContainer.GetContainerState())
            {
                createdPanel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
            }
            
            createdPanel.Open();
        }

        private void OnStartDrag(BaseItemContainerPanel inventoryPanel, int index)
        {
            _itemContextMenu.Hide();
            _itemTooltip.HideTooltip();

            var actualIndex = index; 
            
            if (actualIndex == -1) 
                return; 

            var inventoryItem = _panelsToContainers[inventoryPanel].GetItem(actualIndex);
            if (inventoryItem.IsEmpty)
                return;

            inventoryPanel.CreateDragItem(inventoryItem.Item.Icon, inventoryItem.Amount, actualIndex);
        }

        private void OnClicked(BaseItemContainerPanel panel, int index)
        {
            _itemContextMenu.Hide();
        }

        private void OnDoubleClicked(BaseItemContainerPanel inventoryPanel, int index)
        {
            _itemContextMenu.Hide();

            var inventoryItem = GetItemFromPanel(inventoryPanel, index);
            if (inventoryItem.IsEmpty) return;

            var context = new ItemClickContext(_levelSystem, inventoryItem, index);

            if (ProcessActionsForItem(inventoryPanel, inventoryItem, context))
            {
                _itemTooltip.HideTooltip();
                OnStatsChanged?.Invoke();
            }
        }

        private void OnItemActionRequested(BaseItemContainerPanel inventoryPanel, int index, Vector3 slotPosition)
        {
            var inventoryItem = GetItemFromPanel(inventoryPanel, index);

            var actions = GetAvailableActions(inventoryPanel, inventoryItem);
            _itemTooltip.HideTooltip();
            _itemContextMenu.Show(actions, slotPosition);

            var context = new ItemClickContext(_levelSystem, inventoryItem, index);

            foreach (var activeOption in _itemContextMenu.ActiveOptions)
            {
                activeOption.OnOptionClicked -= OnOptionClicked;
                activeOption.AssignAction(context);
                activeOption.OnOptionClicked += OnOptionClicked;
            }
        }

        private void OnOptionClicked(ItemClickAction action, ItemClickContext context)
        {
            if (action.OnActionClickSuccess(context))
            {
                foreach (var activeOption in _itemContextMenu.ActiveOptions)
                {
                    activeOption.OnOptionClicked -= OnOptionClicked;
                }
                
                OnStatsChanged?.Invoke();
            }
        }

        private InventoryItem GetItemFromPanel(BaseItemContainerPanel inventoryPanel, int index) => 
            _panelsToContainers[inventoryPanel].GetItem(index);

        private bool ProcessActionsForItem(BaseItemContainerPanel inventoryPanel, InventoryItem inventoryItem, ItemClickContext context)
        {
            foreach (var action in _actions)
            {
                if (action.StartPanel == inventoryPanel && inventoryItem.Item.TryGetProperty<Property>(action.ActionType, out _) && action.OnActionClickSuccess(context))
                {
                    return true;
                }
            }
            
            return false;
        }

        private List<ItemClickAction> GetAvailableActions(BaseItemContainerPanel inventoryPanel, InventoryItem inventoryItem)
        {
            var actions = new List<ItemClickAction>();
            foreach (var action in _actions)
            {
                if (action.GetType() == typeof(TransferClickAction) && action.StartPanel == inventoryPanel)
                {
                    actions.Add(action);
                    continue;
                }
                
                if (action.StartPanel == inventoryPanel && inventoryItem.Item.TryGetProperty<Property>(action.ActionType, out _))
                {
                    actions.Add(action);
                }
            }
            return actions;
        }

        private void OnSwapRequested(BaseItemContainerPanel startContainer,
            BaseItemContainerPanel endContainer, 
            int startIndex, int endIndex)
        {
            var isEndConditionMet = ConditionUtils.IsConditionMet(_levelSystem, endContainer.Slots[endIndex],
                _panelsToContainers[startContainer].GetItem(startIndex));
            
            var isStartConditionMet = ConditionUtils.IsConditionMet(_levelSystem, startContainer.Slots[startIndex],
                _panelsToContainers[endContainer].GetItem(endIndex));
            
            if (!isEndConditionMet || !isStartConditionMet)
            {
                if (_panelsToContainers[endContainer].IsFilterActive)
                {
                    _panelsToContainers[endContainer].AddItem(_panelsToContainers[startContainer].GetItem(startIndex));
                    _panelsToContainers[startContainer].RemoveItemsAtIndex(startIndex);
                }

                ShowTooltip(endContainer, endIndex);
                return;
            }

            if (startContainer == endContainer)
            {
                _panelsToContainers[startContainer].SwapItems(startIndex, endIndex);
                
                ShowTooltip(startContainer, endIndex);
            }
            else
            {
                var itemFromStartContainer = _panelsToContainers[startContainer].GetItem(startIndex);
                var itemFromEndContainer = _panelsToContainers[endContainer].GetItem(endIndex);
                _panelsToContainers[startContainer].SetItem(startIndex, itemFromEndContainer);
                _panelsToContainers[endContainer].SetItem(endIndex, itemFromStartContainer);
                
                ShowTooltip(endContainer, endIndex);
            }
        }

        private void HideTooltip(BaseItemContainerPanel panel, int index) => _itemTooltip.HideTooltip();

        private void ShowTooltip(BaseItemContainerPanel panel, int index)
        {
            var item = _panelsToContainers[panel].GetItem(index);
            _itemTooltip.ShowTooltip(item, CollectRequirements(item));
        }

        private void ApplyFilter(BaseItemContainerPanel panel, ItemFilter filter)
        {
            _panelsToContainers[panel].SetFilter(filter);
            OnInventoriesUpdated(_panelsToContainers[panel]);
        }

        private void ResetFilter(BaseItemContainerPanel panel)
        {
            ApplyFilter(panel, null); 
        }

        private void ApplyTypeFilter(BaseItemContainerPanel panel, int type)
        {
            if (type == 0)
            {
                ResetFilter(panel);
            }
            else
            {
                var filter = new TypeFilter((ItemType)(type - 1));
                ApplyFilter(panel, filter);
            }
        }

        private void ApplyItemSearch(BaseItemContainerPanel panel, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                ResetFilter(panel);
            }
            else
            {
                var filter = new NameFilter(searchTerm);
                ApplyFilter(panel, filter);
            }
        }

        private string CollectRequirements(InventoryItem item)
        {
            var sb = new StringBuilder();

            if (item.Item.TryGetProperty<EquippableProperty>(out var equippableProperty))
            {
                if (equippableProperty.Level > 0)
                {
                    string color = _levelSystem.CurrentLevel >= equippableProperty.Level ? Constants.AVAILABLE_COLOR : Constants.UNAVAILABLE_COLOR;
                    sb.Append($"<color={color}>Required level: {equippableProperty.Level}</color>\n");
                }
            }

            return sb.ToString();
        }

        private void OnInventoriesUpdated(ItemContainer itemContainer)
        {
            _itemContextMenu.Hide();

            foreach (var panel in _containerPanels)
            {
                if (_panelsToContainers[panel] == itemContainer)
                {
                    panel.ResetAllItems();

                    foreach (var (index, inventoryItem) in itemContainer.GetContainerState())
                    {
                        panel.UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
                    }
                }
            }

            if (itemContainer == _equipmentContainer)
            {
                _playerModel.RefreshItems(_equipmentContainer.GetContainerState());
                OnStatsChanged?.Invoke();
            }
        }

        private void ClosePanel(BaseItemContainerPanel panel)
        {
            panel.OnStartDrag -= OnStartDrag;
            panel.OnClicked -= OnClicked;
            panel.OnDoubleClicked -= OnDoubleClicked;
            panel.OnSwapRequested -= OnSwapRequested;
            panel.OnItemActionRequested -= OnItemActionRequested;
            panel.OnItemSearchRequested -= ApplyItemSearch;
            panel.OnItemTypeRequested -= ApplyTypeFilter;
            panel.OnShowTooltipRequested -= ShowTooltip;
            panel.OnHideTooltipRequested -= HideTooltip;

            _panelsToContainers[panel].OnItemsUpdated -= OnInventoriesUpdated;

            _actions.RemoveAll(action => action.StartPanel == panel || action.EndPanel == panel);
            _panelsToContainers.Remove(panel);
            _containerPanels.Remove(panel);
            panel.Close();
            _openedPanels.Remove(panel);
        }
    }
}