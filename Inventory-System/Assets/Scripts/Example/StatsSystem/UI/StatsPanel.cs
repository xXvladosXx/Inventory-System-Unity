using System.Collections.Generic;
using Example.StatsSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Example.StatsSystem.UI
{
    public class StatsPanel : SerializedMonoBehaviour
    {
        [SerializeField] private StatRow _statRowPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private int _initialPoolSize = 10;

        private readonly Dictionary<string, StatRow> _activeStatRows = new Dictionary<string, StatRow>(); 
        private readonly Queue<StatRow> _rowPool = new Queue<StatRow>();
        
        public void InitializePool()
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                var row = Instantiate(_statRowPrefab, _content);
                row.gameObject.SetActive(false);
                _rowPool.Enqueue(row);
            }
        }

        public void RefreshStats(Dictionary<StatType, float> changedStats)
        {
            var count = 0;

            foreach (var stat in changedStats)
            {
                var statKey = stat.Key.ToString();
                if (_activeStatRows.TryGetValue(statKey, out var existingRow))
                {
                    existingRow.SetStat(statKey, stat.Value);
                    existingRow.AnimateRowAppearance(count);
                }
                else
                {
                    var row = GetStatRow();
                    row.SetStat(statKey, stat.Value);
                    _activeStatRows[statKey] = row;
                    row.AnimateRowAppearance(count);
                }
                
                count++;
            }
        }

        private StatRow GetStatRow()
        {
            StatRow row;
            
            if (_rowPool.Count > 0)
            {
                row = _rowPool.Dequeue();
                row.gameObject.SetActive(true);
            }
            else
            {
                row = Instantiate(_statRowPrefab, _content);
            }
            
            return row;
        }
    }
}