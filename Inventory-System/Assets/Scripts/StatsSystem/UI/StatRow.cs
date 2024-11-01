using DG.Tweening;
using TMPro;
using UnityEngine;

namespace StatsSystem
{
    public class StatRow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _statText;
        [SerializeField] private Transform _content;
        [SerializeField] private float _animationDelay = 0.05f;
        [SerializeField] private float _duration = .3f;
        [SerializeField] private float _durationFade = .3f;
        [SerializeField] private float _offset = 25;

        public void AnimateRowAppearance(int delay)
        {
            var canvasGroup = _content.GetComponent<CanvasGroup>();
            if (canvasGroup == null) 
            {
                canvasGroup = _content.gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.DOKill();
            _content.DOKill();

            var startPos = transform.position - Vector3.right * _offset;
            _content.transform.position = startPos;
            canvasGroup.alpha = 0;

            _content.transform.DOMoveX(transform.position.x + _offset, _duration)
                .SetEase(Ease.OutQuad)
                .SetDelay(_animationDelay * delay);
            
            canvasGroup.DOFade(1, _durationFade).SetDelay(_animationDelay * delay);
        }
        
        public void SetStat(string stat, float value)
        {
            _statText.text = $"{stat}: {value}";
        }
    }
}