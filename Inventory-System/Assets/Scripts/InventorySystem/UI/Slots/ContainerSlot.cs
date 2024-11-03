using System;
using System.Collections.Generic;
using InventorySystem.Items;
using InventorySystem.UI.Slots.SlotType;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI.Slots
{
    public class ContainerSlot : SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private List<SlotCondition> _conditions = new List<SlotCondition>();
        
        private bool _empty = true;
        private float _lastClickTime;

        public int Index { get; private set; }
        
        public Sprite Icon => _icon.sprite;
        public int Amount => int.Parse(_amount.text);
        public IReadOnlyList<SlotCondition> Conditions => _conditions;
        
        public event Action<ContainerSlot> OnSlotClicked;
        public event Action<ContainerSlot> OnSlotBeginDrag;
        public event Action<ContainerSlot> OnSlotEndDrag;
        public event Action<ContainerSlot> OnSlotDrop;
        public event Action<ContainerSlot> OnSlotRightClicked;
        public event Action<ContainerSlot> OnPointerSlotEnter;
        public event Action<ContainerSlot> OnPointerSlotExit;
        public event Action<ContainerSlot> OnSlotDoubleClicked;

        private const float DOUBLE_CLICK_THRESHOLD = 0.3f;

        public void Initialize(int index)
        {
            Index = index;
        }
        
        public void ResetData()
        {
            _icon.gameObject.SetActive(false);
            _empty = true;
        }
        
        public void SetData(Sprite icon, int amount)
        {
            _icon.sprite = icon;
            
            if (amount == 1)
            {
                _amount.gameObject.SetActive(false);
            }
            else
            {
                _amount.gameObject.SetActive(true);
                _amount.text = amount.ToString();
            }
            
            _icon.gameObject.SetActive(true);
            _empty = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - _lastClickTime < DOUBLE_CLICK_THRESHOLD)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    OnDoubleClick();
                    _lastClickTime = 0;
                }
            }
            else
            {
                _lastClickTime = Time.time; 

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    OnRightClick();
                }
                else
                {
                    OnClick();
                }
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
        
        private void OnDoubleClick()
        {
            if (_empty)
            {
                return;
            }
            
            OnSlotDoubleClicked?.Invoke(this);
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_empty)
            {
                return;
            }

            OnPointerSlotEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_empty)
            {
                return;
            }

            OnPointerSlotExit?.Invoke(this);
        }
    }
}