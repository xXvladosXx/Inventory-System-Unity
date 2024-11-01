using System;
using System.Collections.Generic;
using InventorySystem.Items.Stats;
using InventorySystem.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StatsSystem
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
        public void Calculate()
        {
            var collectedStats = CollectStats();
            var summedStats = SumStats(collectedStats);
            
            _cachedStats.Clear();
            foreach (var baseStatKey in _statsContainer.BaseStats.Keys)
            {
                _cachedStats[baseStatKey] = summedStats.GetValueOrDefault(baseStatKey, 0); 
            }
            
            DisplayStats();
        }

        private void RecalculateStats()
        {
            Calculate();
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

        private void DisplayStats()
        {
            _statsPanel.RefreshStats(_cachedStats);
        }
    }
}