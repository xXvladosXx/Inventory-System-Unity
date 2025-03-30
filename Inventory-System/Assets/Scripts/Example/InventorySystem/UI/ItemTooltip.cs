using DG.Tweening;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using InventorySystem.UI.Slots;
using TMPro;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _canvasRectTransform;
        
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [SerializeField] private TextMeshProUGUI _itemName;         
        [SerializeField] private TextMeshProUGUI _itemCathegory;    
        [SerializeField] private TextMeshProUGUI _itemRequirements;    
        [SerializeField] private TextMeshProUGUI _itemDescription;         

        private Tween _fadeTween;
        private float _hideDelay = 0.2f; 
    
        private void Awake()
        {
            gameObject.SetActive(false);

            _canvasGroup.alpha = 0; 
        }

        public void ShowTooltip(InventoryItem inventoryItem, string requirements)
        {
            _fadeTween?.Kill();

            if (inventoryItem.Item == null)
            {
                HideTooltip();
                return;
            }
        
            gameObject.SetActive(true);
            _itemName.text = inventoryItem.Item.Name;
            
            _itemCathegory.text = inventoryItem.Item.TryGetProperty<EquippableProperty>(out var equippableProperty) 
                ? equippableProperty.EquipType.ToString() : inventoryItem.Item.ItemType.ToString();

            if (requirements != "")
            {
                _itemRequirements.text = requirements;
                _itemRequirements.gameObject.SetActive(true);
            }
            else
            {
                _itemRequirements.gameObject.SetActive(false);
            }
            
            _itemDescription.text = inventoryItem.Item.GetPropertiesDescription();
        
            gameObject.transform.localScale = Vector3.zero;
            UpdateTooltipPosition();
        
            gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack); 
            _fadeTween = _canvasGroup.DOFade(1, 0.2f);
        }

        public void HideTooltip()
        {
            if (!gameObject.activeSelf) return;
        
            _fadeTween?.Kill();

            _fadeTween = _canvasGroup.DOFade(0, 0.2f).OnComplete(() =>
            {
                gameObject.SetActive(false); 
            });
        
            gameObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }

        private void UpdateTooltipPosition()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, 
                Input.mousePosition, 
                _canvas.worldCamera, 
                out localPoint
            );

            Vector2 tooltipPosition = localPoint;
            Vector2 tooltipSize = _rectTransform.sizeDelta;
            Vector2 canvasSize = _canvasRectTransform.sizeDelta;

            if (tooltipPosition.x + tooltipSize.x > canvasSize.x / 2)
            {
                tooltipPosition.x = canvasSize.x / 2 - tooltipSize.x;
            }

            if (tooltipPosition.x < -canvasSize.x / 2)
            {
                tooltipPosition.x = -canvasSize.x / 2;
            }

            if (tooltipPosition.y + tooltipSize.y > canvasSize.y / 2)
            {
                tooltipPosition.y = canvasSize.y / 2 - tooltipSize.y;
            }

            if (tooltipPosition.y < -canvasSize.y / 2)
            {
                tooltipPosition.y = -canvasSize.y / 2;
            }

            _rectTransform.localPosition = tooltipPosition;
        }

        private void Update()
        {
            if (gameObject.activeSelf)
            {
                UpdateTooltipPosition();
            }
        }
    }
}