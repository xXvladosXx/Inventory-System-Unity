using System;
using DG.Tweening;
using InventorySystem.UI.ClickAction;
using TMPro;
using UnityEngine;
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
        public ItemClickAction ContextOption { get; private set; }

        public event Action<ItemClickAction, ItemClickContext> OnOptionClicked; 

        public void SetOption(ItemClickAction contextOption)
        {
            _button.onClick.RemoveAllListeners();
            
            ContextOption = contextOption;
            _text.text = contextOption.Name;
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
            _button.onClick.AddListener(() =>
            {
                OnOptionClicked?.Invoke(ContextOption, itemClickContext);
            });
        }
        
        public void KillTweens()
        {
            _appearanceTween?.Kill();
            _fadeTween?.Kill();
        }
    }
}