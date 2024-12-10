using System.Collections.Generic;
using Example.StatsSystem.Core;
using Example.StatsSystem.Stats;
using Example.StatsSystem.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Example.StatsSystem
{
    public class StatsController : SerializedMonoBehaviour
    {
        [SerializeField] private List<IStatsChangeable> _statsChangeables = new List<IStatsChangeable>();
        [SerializeField] private StatsPanel _statsPanel;
        [SerializeField] private StatsContainer _statsContainer;

        private readonly Dictionary<StatType, float> _cachedStats = new Dictionary<StatType, float>();

        private void Awake()
        {
            _statsPanel.InitializePool();
            RecalculateStats();
            foreach (var changeable in _statsChangeables)
            {
                changeable.OnStatsChanged += RecalculateStats;
            }
        }

        private void OnDestroy()
        {
            foreach (var changeable in _statsChangeables)
            {
                changeable.OnStatsChanged -= RecalculateStats;
            }
        }

        [Button]
        public void CalculateAndUpdateChangedStats()
        {
            var collectedStats = CollectStats();
            var summedStats = SumStats(collectedStats);

            var changedStats = new Dictionary<StatType, float>();

            foreach (var stat in summedStats)
            {
                if (!_cachedStats.TryGetValue(stat.Key, out var cachedValue) || !Mathf.Approximately(cachedValue, stat.Value))
                {
                    changedStats[stat.Key] = stat.Value;
                    _cachedStats[stat.Key] = stat.Value;  
                }
            }

            DisplayChangedStats(changedStats);
        }

        private void RecalculateStats()
        {
            CalculateAndUpdateChangedStats();
        }

        private Dictionary<StatType, List<CoreStat>> CollectStats()
        {
            var collectedStats = new Dictionary<StatType, List<CoreStat>>();
            
            foreach (var changeable in _statsChangeables)
            {
                changeable.CollectStats(collectedStats);
            }

            return collectedStats;
        }

        private Dictionary<StatType, float> SumStats(Dictionary<StatType, List<CoreStat>> collectedStats)
        {
            var summedStats = new Dictionary<StatType, float>();

            foreach (var statList in collectedStats.Values)
            {
                foreach (var coreStat in statList)
                {
                    if (summedStats.ContainsKey(coreStat.StatType))
                    {
                        summedStats[coreStat.StatType] += coreStat.Value;
                    }
                    else
                    {
                        summedStats[coreStat.StatType] = coreStat.Value;
                    }
                }
            }

            return summedStats;
        }

        private void DisplayChangedStats(Dictionary<StatType, float> changedStats)
        {
            if (changedStats.Count > 0)
            {
                var orderedChangedStats = new Dictionary<StatType, float>();

                foreach (var statType in _statsContainer.BaseStats.Keys)
                {
                    if (changedStats.TryGetValue(statType, out float value))
                    {
                        orderedChangedStats[statType] = value;
                    }
                }

                _statsPanel.RefreshStats(orderedChangedStats);
            }
        }
    }
}