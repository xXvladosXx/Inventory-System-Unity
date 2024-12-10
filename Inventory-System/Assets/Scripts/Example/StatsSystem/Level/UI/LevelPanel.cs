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

        private void Awake()
        {
            _levelUp.onClick.AddListener(() =>
            {
                _levelSystem.AddExperience(50);
                RefreshProgress();
            });
            
            RefreshProgress();
        }

        private void RefreshProgress()
        {
            _level.text = $"Level {_levelSystem.CurrentLevel}";
            _progress.fillAmount = (float) _levelSystem.CurrentExperience / _levelSystem.ExperiencePerLevel[_levelSystem.CurrentLevel + 1];
        }
    }
}