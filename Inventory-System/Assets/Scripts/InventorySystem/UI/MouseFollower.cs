using System;
using UnityEngine;

namespace InventorySystem.UI
{
    public class MouseFollower : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private InventorySlot _slot;
        
        public Sprite Icon => _slot.Icon;
        public int Amount => _slot.Amount;

        public void SetData(Sprite icon, int amount)
        {
            _slot.SetData(icon, amount);
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