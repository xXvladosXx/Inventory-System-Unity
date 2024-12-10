using System;
using System.Collections.Generic;
using Example.StatsSystem.Stats;

namespace Example.StatsSystem.Core
{
    public interface IStatsChangeable
    {
        event Action OnStatsChanged;
        Dictionary<StatType, List<CoreStat>> CollectStats(Dictionary<StatType, List<CoreStat>> stats);
    }
}