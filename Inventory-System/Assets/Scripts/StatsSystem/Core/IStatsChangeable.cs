using System;
using System.Collections.Generic;
using InventorySystem.Items.Stats;

namespace StatsSystem
{
    public interface IStatsChangeable
    {
        event Action OnStatsChanged;
        Dictionary<StatType, List<CoreStat>> CollectStats(Dictionary<StatType, List<CoreStat>> stats);
    }
}