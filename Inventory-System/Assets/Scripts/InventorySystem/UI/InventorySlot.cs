using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amount;

        private bool _empty = true;
        
        public event Action<InventorySlot> OnSlotClicked; 
        public event Action<InventorySlot> OnSlotBeginDrag; 
        public event Action<InventorySlot> OnSlotEndDrag; 
        public event Action<InventorySlot> OnSlotDrop; 
        public event Action<InventorySlot> OnSlotRightClicked;
        
        public Sprite Icon => _icon.sprite;
        public int Amount => int.Parse(_amount.text);

        public void Awake()
        {
            ResetData();
        }

        private void ResetData()
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