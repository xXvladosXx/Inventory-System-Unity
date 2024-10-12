using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public abstract class BaseItemContainerPanel : MonoBehaviour
    {
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected MouseFollower _mouseFollower;
        [SerializeField] protected List<ContainerSlot> _slots = new List<ContainerSlot>();

        public ItemContainer ItemContainer { get; protected set; }

        public event Action<int> OnItemActionRequested;
        public event Action<BaseItemContainerPanel, int> OnStartDrag;
        public event Action<ItemContainer, ItemContainer, int, int> OnSwapRequested;

        public void Initialize(ItemContainer itemContainer)
        {
            ItemContainer = itemContainer;
            InitializeSlots();
            _mouseFollower.Toggle(false);
        }

        protected abstract void InitializeSlots();
        public virtual void DisposeSlots()
        {
            foreach (var slot in _slots)
            {
                Destroy(slot.gameObject);
            }
            
            _slots.Clear();
        }

        protected virtual void AddSlotListeners(ContainerSlot slot)
        {
            slot.OnSlotClicked += OnSlotClicked;
            slot.OnSlotBeginDrag += OnSlotBeginDrag;
            slot.OnSlotEndDrag += OnSlotEndDrag;
            slot.OnSlotDrop += OnSlotDrop;
            slot.OnSlotRightClicked += OnSlotRightClicked;
        }

        protected virtual void RemoveSlotListeners(ContainerSlot slot)
        {
            slot.OnSlotClicked -= OnSlotClicked;
            slot.OnSlotBeginDrag -= OnSlotBeginDrag;
            slot.OnSlotEndDrag -= OnSlotEndDrag;
            slot.OnSlotDrop -= OnSlotDrop;
            slot.OnSlotRightClicked -= OnSlotRightClicked;
        }

        private void OnDestroy()
        {
            foreach (var slot in _slots)
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
            int index = _slots.IndexOf(slot);
            if (index == -1)
                return;

            CreateDragItem(slot.Icon, slot.Amount, index);
            OnStartDrag?.Invoke(this, index);
        }

        protected virtual void OnSlotEndDrag(ContainerSlot slot)
        {
            ResetDragItem();
        }

        protected virtual void OnSlotDrop(ContainerSlot slot)
        {
            var index = _slots.IndexOf(slot);
            if (index == -1)
            {
                ResetDragItem();
                return;
            }

            if (!slot.IsConditionMet(_mouseFollower.ItemContainer.GetItem(_mouseFollower.StartIndex)))
            {
                ResetDragItem();
                return;
            }

            OnSwapRequested?.Invoke(_mouseFollower.ItemContainer, ItemContainer, _mouseFollower.StartIndex, index);
        }

        protected virtual void OnSlotRightClicked(ContainerSlot slot)
        {
        }

        public void UpdateSlot(int index, Sprite icon, int amount)
        {
            _slots[index].SetData(icon, amount);
        }

        public void CreateDragItem(Sprite itemIcon, int inventoryItemAmount, int index)
        {
            _mouseFollower.SetData(this, itemIcon, inventoryItemAmount, index);
            _mouseFollower.Toggle(true);
        }

        protected void ResetDragItem()
        {
            _mouseFollower.Toggle(false);
        }

        public void ResetAllItems()
        {
            foreach (var slot in _slots)
            {
                slot.ResetData();
            }
        }
    }
}
