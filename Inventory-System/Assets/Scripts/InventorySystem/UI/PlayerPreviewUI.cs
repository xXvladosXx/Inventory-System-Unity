using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class PlayerPreviewUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private Button _rightRotate;
        [SerializeField] private Button _leftRotate;
        
        private EquipmentPreview _model;
        
        public bool IsPointerInside { get; private set; }
        public bool IsHolding { get; private set; }

        public void Initialize(EquipmentPreview model)
        {
            _model = model;
        }
        
        private void Update()
        {
            if (IsHolding)
            {
                float horizontalInput = Input.GetAxis("Mouse X");
                _model.transform.Rotate(Vector3.up, -horizontalInput * _rotationSpeed * Time.deltaTime);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerInside = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsPointerInside)
                IsHolding = true;
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            IsHolding = false;
        }
    }
}