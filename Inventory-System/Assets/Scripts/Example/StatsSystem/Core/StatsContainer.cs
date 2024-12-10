using System;
using System.Collections.Generic;
using Example.StatsSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Example.StatsSystem.Core
{
    public class StatsContainer : SerializedMonoBehaviour, IStatsChangeable
    {
        [SerializeField] private Dictionary<StatType, List<CoreStat>> _baseStats = new Dictionary<StatType, List<CoreStat>>();

        public IReadOnlyDictionary<StatType, List<CoreStat>> BaseStats => _baseStats;
        
        public event Action OnStatsChanged;
        public Dictionary<StatType, List<CoreStat>> CollectStats(Dictionary<StatType, List<CoreStat>> stats)
        {
            foreach (var kvp in _baseStats)
            {
                if (!stats.ContainsKey(kvp.Key))
                {
                    stats[kvp.Key] = new List<CoreStat>();
                }
                
                stats[kvp.Key].AddRange(kvp.Value);
            }

            return stats;
        }
    }
}