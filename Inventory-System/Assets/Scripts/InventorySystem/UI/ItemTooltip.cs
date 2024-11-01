using DG.Tweening;
using InventorySystem.Items;
using TMPro;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _itemName;         
        [SerializeField] private TextMeshProUGUI _itemCathegory;    
        [SerializeField] private TextMeshProUGUI _itemDescription;         

        public GameObject tooltipPanel;  
        public Vector2 offset;           

        private RectTransform tooltipRectTransform; 
        private Canvas canvas;                      

        private Tween fadeTween;
        private float hideDelay = 0.2f; 
    
        private void Awake()
        {
            tooltipPanel.SetActive(false);

            tooltipRectTransform = tooltipPanel.GetComponent<RectTransform>();
            canvas = tooltipPanel.GetComponentInParent<Canvas>();
            tooltipPanel.GetComponent<CanvasGroup>().alpha = 0; 
        }

        public void ShowTooltip(InventoryItem inventoryItem)
        {
            fadeTween?.Kill();

            if (inventoryItem.Item == null)
            {
                HideTooltip();
                return;
            }
        
            tooltipPanel.SetActive(true);
            _itemName.text = inventoryItem.Item.Name;
            _itemCathegory.text = inventoryItem.Item.ItemType.ToString();
            _itemDescription.text = inventoryItem.Item.GetPropertiesDescription();
        
            tooltipPanel.transform.localScale = Vector3.zero;
            UpdateTooltipPosition();
        
            tooltipPanel.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack); 
            fadeTween = tooltipPanel.GetComponent<CanvasGroup>().DOFade(1, 0.2f);
        }

        public void HideTooltip()
        {
            if (!tooltipPanel.activeSelf) return;
        
            fadeTween?.Kill();

            fadeTween = tooltipPanel.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(() =>
            {
                tooltipPanel.SetActive(false); 
            });
        
            tooltipPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }

        private void UpdateTooltipPosition()
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                Input.mousePosition, 
                canvas.worldCamera, 
                out localPoint
            );

            Vector2 tooltipPosition = localPoint + offset;

            Vector2 tooltipSize = tooltipRectTransform.sizeDelta;

            Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

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

            tooltipRectTransform.localPosition = tooltipPosition;
        }

        private void Update()
        {
            if (tooltipPanel.activeSelf)
            {
                UpdateTooltipPosition();
            }
        }
    }
}