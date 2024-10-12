using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.UI.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class ContainerSlot : SerializedMonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private List<SlotCondition> _conditions = new List<SlotCondition>();
        
        private bool _empty = true;
        
        public event Action<ContainerSlot> OnSlotClicked; 
        public event Action<ContainerSlot> OnSlotBeginDrag; 
        public event Action<ContainerSlot> OnSlotEndDrag; 
        public event Action<ContainerSlot> OnSlotDrop; 
        public event Action<ContainerSlot> OnSlotRightClicked;
        
        public Sprite Icon => _icon.sprite;
        public int Amount => int.Parse(_amount.text);

        public void ResetData()
        {
            _icon.gameObject.SetActive(false);
            _empty = true;
        }
        
        public void SetData(Sprite icon, int amount)
        {
            _icon.sprite = icon;
            _amount.text = amount.ToString();
            _icon.gameObject.SetActive(true);
            _empty = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick();
            }
            else
            {
                OnClick();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_empty)
            {
                return;
            }
            
            OnSlotBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnSlotEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnSlotDrop?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData) { }

        public bool IsConditionMet(InventoryItem item)
        {
            if (_conditions.Count == 0)
                return true;
            
            foreach (var condition in _conditions)
            {
                if (condition.IsMet(item))
                {
                    return true;
                }
            }

            return false;
        }
        
        private void OnClick()
        {
            if (_empty)
            {
                return;
            }
            
            OnSlotClicked?.Invoke(this);
        }

        private void OnRightClick()
        {
            if (_empty)
            {
                return;
            }
            
            OnSlotRightClicked?.Invoke(this);
        }
    }
}