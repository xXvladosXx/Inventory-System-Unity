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
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI.Panels
{
    public abstract class BaseItemContainerPanel : SerializedMonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [field: SerializeField] public string ContainerName { get; private set; }
        
        [SerializeField] protected List<ContainerSlot> slots = new List<ContainerSlot>();
        [SerializeField] private DragItemCreator _dragItemCreator;
        [SerializeField] private float edgeMargin = 10;
        [SerializeField] private bool _isDraggable = false;
        
        private bool _isDragging;
        private Vector2 _startDragPosition;
        private RectTransform _rectTransform;

        public IReadOnlyList<ContainerSlot> Slots => slots;
        
        public event Action<BaseItemContainerPanel, int, Vector3> OnItemActionRequested;
        public event Action<BaseItemContainerPanel, int> OnStartDrag;
        public event Action<BaseItemContainerPanel, int> OnClicked;
        public event Action<BaseItemContainerPanel, int> OnDoubleClicked;
        public event Action<BaseItemContainerPanel, BaseItemContainerPanel, int, int> OnSwapRequested;
        public event Action<BaseItemContainerPanel, string> OnItemSearchRequested;
        public event Action<BaseItemContainerPanel, int> OnItemTypeRequested;
        public event Action<BaseItemContainerPanel, int> OnShowTooltipRequested;
        public event Action<BaseItemContainerPanel, int> OnHideTooltipRequested;
        private Vector2 _initialPanelPosition;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(int size)
        {
            _rectTransform = GetComponent<RectTransform>();

            InitializeSlots(size);
            InitializeFilters();
            
            var index = 0;
            foreach (var slot in slots)
            {
                slot.Initialize(index);
                index++;
            }
            
            _dragItemCreator.Toggle(false);
        }

        protected abstract void InitializeSlots(int size);

        protected virtual void InitializeFilters() { }

        protected virtual void DisposeFilters() { }

        public virtual void DisposeSlots()
        {
            DisposeFilters();
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
            OnClicked?.Invoke(this, slot.Index);
        }

        protected virtual void OnSlotBeginDrag(ContainerSlot slot)
        {
            if (slot.Index == -1)
                return;

            OnStartDrag?.Invoke(this, slot.Index);
        }

        protected virtual void OnSlotDoubleClicked(ContainerSlot slot)
        {
            int index = slot.Index;
            if (index == -1)
                return;

            OnDoubleClicked?.Invoke(this, slot.Index);
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
            
            OnSwapRequested?.Invoke(_dragItemCreator.InventoryPanel, this, _dragItemCreator.StartIndex, slot.Index);
        }

        protected virtual void OnSlotRightClicked(ContainerSlot slot)
        {
            OnItemActionRequested?.Invoke(this, slot.Index, slot.transform.position);
        }

        private void OnSlotPointerExit(ContainerSlot slot)
        {
            OnHideTooltipRequested?.Invoke(this, slot.Index);
        }

        private void OnSlotPointerEnter(ContainerSlot slot)
        {
            if (_dragItemCreator.IsDragging)
                return;

            if (slot.Index == -1)
            {
                OnHideTooltipRequested?.Invoke(this, slot.Index);
            }
            else
            {
                OnShowTooltipRequested?.Invoke(this, slot.Index);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;
            
            if (IsPointerOnEdge(eventData))
            {
                _isDragging = true;
                transform.SetAsLastSibling();

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _rectTransform.parent as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera,
                    out _startDragPosition
                );

                _initialPanelPosition = _rectTransform.anchoredPosition;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;

            _isDragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggable)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var currentPointerPosition
            );

            Vector2 offset = currentPointerPosition - _startDragPosition;
            _rectTransform.anchoredPosition = _initialPanelPosition + offset;
        }

        private bool IsPointerOnEdge(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position,
                eventData.pressEventCamera, out var localPointerPosition);

            Rect panelRect = _rectTransform.rect;

            return localPointerPosition.x < panelRect.xMin + edgeMargin ||
                   localPointerPosition.x > panelRect.xMax - edgeMargin ||
                   localPointerPosition.y < panelRect.yMin + edgeMargin ||
                   localPointerPosition.y > panelRect.yMax - edgeMargin;
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

        protected void OnOnItemSearchRequested(string itemName) => 
            OnItemSearchRequested?.Invoke(this, itemName);

        protected void OnOnItemTypeRequested(int type) => 
            OnItemTypeRequested?.Invoke(this, type);
    }
}
