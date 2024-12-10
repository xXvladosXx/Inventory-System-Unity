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
        private Quaternion _targetRotation;

        public bool IsPointerInside { get; private set; }
        public bool IsHolding { get; private set; }

        public void Initialize(EquipmentPreview model)
        {
            _model = model;
        }
        
        void Update()
        {
            float horizontalInput = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(horizontalInput) > 0.01f) 
            {
                _targetRotation *= Quaternion.Euler(0, -horizontalInput * _rotationSpeed * Time.deltaTime, 0);
            }

            _model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, _targetRotation, Time.deltaTime * _rotationSpeed);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsHolding)
                return;
            
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