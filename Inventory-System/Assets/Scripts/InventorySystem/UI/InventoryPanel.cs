using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.UI
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] private InventorySlot _inventorySlotPrefab;
        [SerializeField] private RectTransform _content;
        [SerializeField] private MouseFollower _mouseFollower;
        
        private List<InventorySlot> _slots = new List<InventorySlot>();
        private int _currentDragIndex = -1;
        
        public event Action<int> OnItemActionRequested;
        public event Action<int> OnStartDrag;
        public event Action<int, int> OnSwapRequested; 
        
        public void Initialize(int size)
        {
            for (int i = 0; i < size; i++)
            {
                InventorySlot slot = Instantiate(_inventorySlotPrefab, _content);
                _slots.Add(slot);
                
                slot.OnSlotClicked += OnSlotClicked;
                slot.OnSlotBeginDrag += OnSlotBeginDrag;
                slot.OnSlotEndDrag += OnSlotEndDrag;
                slot.OnSlotDrop += OnSlotDrop;  
                slot.OnSlotRightClicked += OnSlotRightClicked;
            }
            
            _mouseFollower.Toggle(false);
        }

        private void OnDestroy()
        {
            foreach (var slot in _slots)
            {
                slot.OnSlotClicked -= OnSlotClicked;
                slot.OnSlotBeginDrag -= OnSlotBeginDrag;
                slot.OnSlotEndDrag -= OnSlotEndDrag;
                slot.OnSlotDrop -= OnSlotDrop;  
                slot.OnSlotRightClicked -= OnSlotRightClicked;
            }
        }

        private void OnSlotClicked(InventorySlot slot)
        {
            
        }

        private void OnSlotBeginDrag(InventorySlot slot)
        {
            int index = _slots.IndexOf(slot);
            if (index == -1)
                return;

            _currentDragIndex = index;
            
            CreateDragItem(slot);
            OnStartDrag?.Invoke(index);
        }

        private void OnSlotEndDrag(InventorySlot slot)
        {
            ResetDragItem();
        }

        private void OnSlotDrop(InventorySlot slot)
        {
            var index = _slots.IndexOf(slot);
            if (index == -1)
            {
                ResetDragItem();
                return;
            }

            OnSwapRequested?.Invoke(_currentDragIndex, index);
        }

        private void OnSlotRightClicked(InventorySlot slot)
        {
            
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDragItem();
        }

        public void UpdateSlot(int index, Sprite icon, int amount)
        {
            _slots[index].SetData(icon, amount);
        }

        private void CreateDragItem(InventorySlot slot)
        {
            _mouseFollower.SetData(slot.Icon, slot.Amount);
            _mouseFollower.Toggle(true);
        }

        private void ResetDragItem()
        {
            _mouseFollower.Toggle(false);
            _currentDragIndex = -1;
        }
    }
}