using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items;
using InventorySystem.Items.Types;
using InventorySystem.UI.ClickAction;
using InventorySystem.UI.Filter;
using InventorySystem.UI.Slots;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI.Panels
{
    public abstract class BaseItemContainerPanel : SerializedMonoBehaviour
    {
        [SerializeField] protected List<ContainerSlot> slots = new List<ContainerSlot>();
        [SerializeField] private DragItemCreator _dragItemCreator;
        [SerializeField] private Dictionary<ActionType, ItemClickAction> _possibleContexts = new Dictionary<ActionType,  ItemClickAction>();

        [SerializeField] private TMP_Dropdown _itemTypeDropdown;
        [SerializeField] private TMP_InputField _searchInputField;

        private ItemFilterer _itemFilterer;
        public ItemContainer ItemContainer { get; private set; }
        public ItemTooltip ItemTooltip { get; private set; }
        
        public IReadOnlyDictionary<ActionType, ItemClickAction> ItemContexts => _possibleContexts;
        public IReadOnlyList<ContainerSlot> Slots => slots;
        
        public event Action<BaseItemContainerPanel, int, Vector3> OnItemActionRequested;
        public event Action<BaseItemContainerPanel, int> OnStartDrag;
        public event Action<BaseItemContainerPanel, int> OnDoubleClicked;
        public event Action<BaseItemContainerPanel, BaseItemContainerPanel, int, int> OnSwapRequested;
        
        public void Initialize(ItemContainer itemContainer, 
            ItemTooltip itemTooltip)
        {
            ItemTooltip = itemTooltip;
            ItemContainer = itemContainer;
            
            InitializeSlots();
            
            var index = 0;
            foreach (var slot in slots)
            {
                slot.Initialize(index);
                index++;
            }
            
            _dragItemCreator.Toggle(false);

            _itemFilterer = new ItemFilterer(ItemContainer);
            
            SetupFilters();
        }

        public void UpdateInventoryDisplay(List<InventoryItem> items = null)
        {
            ResetAllItems();
        
            if (items != null)
            {
                foreach (var (index, inventoryItem) in items.Select((item, index) => (index, item)))
                {
                    UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
                }
            }
            else
            {
                foreach (var (index, inventoryItem) in ItemContainer.GetContainerState())
                {
                    UpdateSlot(index, inventoryItem.Item.Icon, inventoryItem.Amount);
                }
            }
        }


        protected abstract void InitializeSlots();

        public virtual void DisposeSlots()
        {
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }
            
            slots.Clear();
        }

        protected virtual void AddSlotListeners(ContainerSlot slot)
        {
            slot.OnSlotClicked += OnSlotClicked;
            slot.OnSlotDoubleClicked += OnSlotDoubleClicked;
            slot.OnSlotBeginDrag += OnSlotBeginDrag;
            slot.OnSlotEndDrag += OnSlotEndDrag;
            slot.OnSlotDrop += OnSlotDrop;
            slot.OnSlotRightClicked += OnSlotRightClicked;
            slot.OnPointerSlotEnter += OnSlotPointerEnter;
            slot.OnPointerSlotExit += OnSlotPointerExit;
        }

        protected virtual void RemoveSlotListeners(ContainerSlot slot)
        {
            slot.OnSlotClicked -= OnSlotClicked;
            slot.OnSlotDoubleClicked -= OnSlotDoubleClicked;
            slot.OnSlotBeginDrag -= OnSlotBeginDrag;
            slot.OnSlotEndDrag -= OnSlotEndDrag;
            slot.OnSlotDrop -= OnSlotDrop;
            slot.OnSlotRightClicked -= OnSlotRightClicked;
            slot.OnPointerSlotEnter -= OnSlotPointerEnter;
            slot.OnPointerSlotExit -= OnSlotPointerExit;
        }

        private void OnDestroy()
        {
            foreach (var slot in slots)
            {
                RemoveSlotListeners(slot);
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnSlotClicked(ContainerSlot slot)
        {
        }

        protected virtual void OnSlotBeginDrag(ContainerSlot slot)
        {
            if (slot.Index == -1)
                return;

            OnStartDrag?.Invoke(this, _itemFilterer.GetActualIndex(slot.Index));
        }

        protected virtual void OnSlotDoubleClicked(ContainerSlot slot)
        {
            int index = slot.Index;
            if (index == -1)
                return;

            OnDoubleClicked?.Invoke(this, _itemFilterer.GetActualIndex(slot.Index));
        }

        protected virtual void OnSlotEndDrag(ContainerSlot slot)
        {
            ResetDragItem();
        }

        protected virtual void OnSlotDrop(ContainerSlot slot)
        {
            var index = slot.Index;
            if (index == -1)
            {
                ResetDragItem();
                return;
            }
            
            OnSwapRequested?.Invoke(_dragItemCreator.InventoryPanel, this, _dragItemCreator.StartIndex, _itemFilterer.GetActualIndex(slot.Index));
            ItemTooltip.ShowTooltip(ItemContainer.GetItem(_itemFilterer.GetActualIndex(slot.Index)));
        }

        protected virtual void OnSlotRightClicked(ContainerSlot slot)
        {
            ItemTooltip.HideTooltip();
            var index = slot.Index;
            OnItemActionRequested?.Invoke(this, _itemFilterer.GetActualIndex(slot.Index), slot.transform.position);
        }

        private void OnSlotPointerExit(ContainerSlot slot)
        {
            ItemTooltip.HideTooltip();
        }

        private void OnSlotPointerEnter(ContainerSlot slot)
        {
            if (_dragItemCreator.IsDragging)
                return;
            
            var index = slot.Index;
            ItemTooltip.ShowTooltip(ItemContainer.GetItem(_itemFilterer.GetActualIndex(slot.Index)));
        }

        public void UpdateSlot(int index, Sprite icon, int amount)
        {
            slots[index].SetData(icon, amount);
        }

        public void CreateDragItem(Sprite itemIcon, int inventoryItemAmount, int index)
        {
            _dragItemCreator.SetData(this, itemIcon, inventoryItemAmount, index);
            _dragItemCreator.Toggle(true);
        }

        public int GetIndexOfSlot(ContainerSlot slot) => slot.Index;

        public void ResetAllItems()
        {
            foreach (var slot in slots)
            {
                slot.ResetData();
            }
        }

        private void ResetDragItem()
        {
            _dragItemCreator.Toggle(false);
        }

        public void RefreshFilter()
        {
          
        }

        private void SetupFilters()
        {
            SetupTypeFilter();
            SetupSearchFilter();
        }
        
        private void SetupTypeFilter()
        {
            if (_itemTypeDropdown == null) 
                return;

            _itemTypeDropdown.onValueChanged.AddListener(type =>
            {
                if (type == 0)
                {
                    ResetFilters();
                }
                else
                {
                    ClearSearchField();
                    ApplyTypeFilter(type - 1);
                }
            });
        }

        private void SetupSearchFilter()
        {
            if (_searchInputField == null)
                return;

            _searchInputField.onValueChanged.AddListener(searchTerm =>
            {
                if (string.IsNullOrEmpty(searchTerm))
                {
                    ResetFilters();
                }
                else
                {
                    ClearTypeDropdown();
                    ApplySearchFilter(searchTerm);
                }
            });
        }
        
        private void ResetFilters()
        {
            _itemFilterer.ResetFilter();
            UpdateInventoryDisplay(null);
        }

        private void ApplyTypeFilter(int typeIndex)
        {
            var selectedItemType = (ItemType)typeIndex;
            var filteredItems = _itemFilterer.ApplyFilter(new TypeFilter(selectedItemType));
            UpdateInventoryDisplay(filteredItems);
        }

        private void ApplySearchFilter(string searchTerm)
        {
            if (searchTerm == string.Empty)
                return;
            
            var filteredItems = _itemFilterer.ApplyFilter(new NameFilter(searchTerm));
            UpdateInventoryDisplay(filteredItems);
        }

        private void ClearSearchField()
        {
            if (_searchInputField != null)
                _searchInputField.text = string.Empty;
        }

        private void ClearTypeDropdown()
        {
            if (_itemTypeDropdown != null)
                _itemTypeDropdown.value = 0;
        }
    }
}
