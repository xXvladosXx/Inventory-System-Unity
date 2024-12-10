using System;
using DG.Tweening;
using InventorySystem.UI.ClickAction;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InventorySystem.UI.ContextMenu
{
    public class ItemContextOption : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Tween _appearanceTween;
        private Tween _fadeTween;
        
        private ItemClickContext _itemClickContext;
        private ItemClickAction _contextOption;

        public event Action<ItemClickAction, ItemClickContext> OnOptionClicked; 

        public void SetOption(ItemClickAction contextOption)
        {
            _button.onClick.RemoveAllListeners();
            
            _contextOption = contextOption;
            _text.text = contextOption.ActionName;
            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1, 0.2f).SetEase(Ease.InOutSine);
        }

        public void AnimateAppearance(float duration, float delay)
        {
            var canvasGroup = gameObject.GetComponent<CanvasGroup>();
            _appearanceTween = transform.DOScale(1, duration).SetEase(Ease.OutBack).SetDelay(delay);
            _fadeTween = canvasGroup.DOFade(1, duration).SetEase(Ease.InOutSine).SetDelay(delay);
        }

        public void AssignAction(ItemClickContext itemClickContext)
        {
            _itemClickContext = itemClickContext;
            _button.onClick.AddListener(InvokeClickAction);
        }

        private void InvokeClickAction()
        {
            OnOptionClicked?.Invoke(_contextOption, _itemClickContext);
        }

        public void KillTweens()
        {
            _button.onClick.RemoveAllListeners();
            _appearanceTween?.Kill();
            _fadeTween?.Kill();
        }
    }
}