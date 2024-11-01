using System;
using System.Collections.Generic;
using InventorySystem.Items.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StatsSystem
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