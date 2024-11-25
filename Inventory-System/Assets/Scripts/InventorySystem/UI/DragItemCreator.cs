using System;
using InventorySystem.UI.Panels;
using InventorySystem.UI.Slots;
using UnityEngine;

namespace InventorySystem.UI
{
    public class DragItemCreator : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ContainerSlot _slot;
        
        public BaseItemContainerPanel InventoryPanel { get; private set; }
        public int StartIndex { get; private set; } = -1;
        public bool IsDragging => _slot.gameObject.activeSelf;

        public void SetData(BaseItemContainerPanel panel, Sprite itemIcon, 
            int itemAmount, int index)
        {
            InventoryPanel = panel;
            StartIndex = index;
            
            _slot.SetData(itemIcon, itemAmount);
        }

        private void Update()
        {
            if (_slot == null)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out var position);
            _slot.transform.position = _canvas.transform.TransformPoint(position);
        }
        
        public void Toggle(bool value)
        {
            _slot.gameObject.SetActive(value);
        }
    }
}