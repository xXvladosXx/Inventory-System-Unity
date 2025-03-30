using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Example.StatsSystem.Level.UI
{
    public class LevelPanel : MonoBehaviour
    {
        [SerializeField] private LevelSystem _levelSystem;
        [SerializeField] private Image _progress;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private Button _levelUp;
        [SerializeField] private float _fillDuration = 0.5f;

        private void Awake()
        {
            _levelUp.onClick.AddListener(() =>
            {
                _levelSystem.AddExperience(50);
                RefreshProgress();
            });

            RefreshProgress();

            _levelSystem.OnLevelUp += OnLevelUp;
        }

        private void OnDestroy()
        {
            _levelSystem.OnLevelUp -= OnLevelUp;
        }

        private void RefreshProgress()
        {
            _level.text = $"Level {_levelSystem.CurrentLevel}";

            float targetFill = (float) _levelSystem.CurrentExperience /
                               _levelSystem.ExperiencePerLevel[_levelSystem.CurrentLevel + 1];

            _progress.DOFillAmount(targetFill, _fillDuration)
                .SetEase(Ease.OutCubic);
        }

        private void OnLevelUp(int i)
        {
            _level.rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
        }
    }
}