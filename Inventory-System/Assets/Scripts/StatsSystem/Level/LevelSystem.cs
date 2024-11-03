using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace StatsSystem.Level
{
    public class LevelSystem : SerializedMonoBehaviour
{
    [SerializeField] private AnimationCurve _expCurve = AnimationCurve.Linear(1, 100, 100, 5000);
    [SerializeField] private int _maxLevel = 100;
    [SerializeField] private Dictionary<int, int> _experiencePerLevel = new Dictionary<int, int>();
    
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private int _currentExperience = 0;

    public event Action<int> OnLevelUp;

    public int CurrentLevel => _currentLevel;
    public int CurrentExperience => _currentExperience;
    public IReadOnlyDictionary<int, int> ExperiencePerLevel => _experiencePerLevel;

    private void Start()
    {
        InitializeExperienceLevels();
    }

    private void InitializeExperienceLevels()
    {
        _experiencePerLevel.Clear();

        for (int i = 1; i <= _maxLevel; i++)
        {
            int xpRequired = Mathf.FloorToInt(_expCurve.Evaluate(i));
            _experiencePerLevel[i] = xpRequired;
        }
    }

    public void AddExperience(int amount)
    {
        _currentExperience += amount;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        while (_currentLevel < _maxLevel && _currentExperience >= GetExperienceForLevel(_currentLevel + 1))
        {
            _currentExperience -= GetExperienceForLevel(_currentLevel + 1);
            _currentLevel++;
            OnLevelUp?.Invoke(_currentLevel);
        }
    }

    private int GetExperienceForLevel(int level)
    {
        return _experiencePerLevel.ContainsKey(level) ? _experiencePerLevel[level] : int.MaxValue;
    }

    public void ResetLevelSystem()
    {
        _currentLevel = 1;
        _currentExperience = 0;
    }

    [Button]
    public void FillExperienceLevelsFromCurve()
    {
        InitializeExperienceLevels();
    }

    [Button]
    public void DebugAddExperience(int amount)
    {
        AddExperience(amount);
    }
}
}