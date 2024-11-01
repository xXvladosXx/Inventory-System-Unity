using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.Items.Types;
using InventorySystem.UI.ClickAction;
using InventorySystem.UI.Slots;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem.UI.Panels
{
    public abstract class BaseItemContainerPanel : SerializedMonoBehaviour
    {
        [SerializeField] protected List<ContainerSlot> slots = new List<ContainerSlot>();
        
        [SerializeField] private DragItemCreator _dragItemCreator;
        [SerializeField] private Dictionary<ActionType, ItemClickAction> _possibleContexts = new Dictionary<ActionType,  ItemClickAction>();

        public ItemContainer ItemContainer { get; private set; }
        public ItemTooltip ItemTooltip { get; private set; }
        
        public IReadOnlyDictionary<ActionType, ItemClickAction> ItemContexts => _possibleContexts;
        public IReadOnlyList<ContainerSlot> Slots => slots;
        
        public event Action<BaseItemContainerPanel, int> OnItemActionRequested;
        public event Action<BaseItemContainerPanel, int> OnStartDrag;
        public event Action<BaseItemContainerPanel, int> OnDoubleClicked;
        public event Action<BaseItemContainerPanel, BaseItemContainerPanel, int, int> OnSwapRequested;

        public void Initialize(ItemContainer itemContainer, 
            ItemTooltip itemTooltip)
        {
            ItemTooltip = itemTooltip;
            ItemContainer = itemContainer;
            
            InitializeSlots();
            _dragItemCreator.Toggle(false);
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
            int index = slots.IndexOf(slot);
            if (index == -1)
                return;

            CreateDragItem(slot.Icon, slot.Amount, index);
            OnStartDrag?.Invoke(this, index);
        }

        protected virtual void OnSlotDoubleClicked(ContainerSlot slot)
        {
            int index = slots.IndexOf(slot);
            if (index == -1)
                return;

            OnDoubleClicked?.Invoke(this, index);
        }

        protected virtual void OnSlotEndDrag(ContainerSlot slot)
        {
            ResetDragItem();
        }

        protected virtual void OnSlotDrop(ContainerSlot slot)
        {
            var index = slots.IndexOf(slot);
            if (index == -1)
            {
                ResetDragItem();
                return;
            }
            
            ItemTooltip.ShowTooltip(_dragItemCreator.ItemContainer.GetItem(_dragItemCreator.StartIndex));
            OnSwapRequested?.Invoke(_dragItemCreator.InventoryPanel, this, _dragItemCreator.StartIndex, index);
        }

        protected virtual void OnSlotRightClicked(ContainerSlot slot)
        {
            ItemTooltip.HideTooltip();
            var index = slots.IndexOf(slot);
            OnItemActionRequested?.Invoke(this, index);
        }

        private void OnSlotPointerExit(ContainerSlot slot)
        {
            ItemTooltip.HideTooltip();
        }

        private void OnSlotPointerEnter(ContainerSlot slot)
        {
            if (_dragItemCreator.IsDragging)
                return;
            
            var index = slots.IndexOf(slot);
            ItemTooltip.ShowTooltip(ItemContainer.GetItem(index));
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

        public int GetIndexOfSlot(ContainerSlot slot) => slots.IndexOf(slot);

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
    }
}
