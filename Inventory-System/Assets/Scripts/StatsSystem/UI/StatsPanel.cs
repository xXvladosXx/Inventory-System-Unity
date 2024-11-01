using System;
using System.Collections.Generic;
using InventorySystem.Items.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace StatsSystem
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

        public void RefreshStat(string stat, float value)
        {
            var count = 0;
            if (_activeStatRows.TryGetValue(stat, out var existingRow))
            {
                existingRow.SetStat(stat, value);
                existingRow.AnimateRowAppearance(count);
                count++;
            }
            else
            {
                var row = GetStatRow();
                row.SetStat(stat, value);
                _activeStatRows[stat] = row;
                row.AnimateRowAppearance(count);
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

        public void RefreshStats(Dictionary<StatType,float> stats)
        {
            var count = 0;

            foreach (var stat in stats)
            {
                if (_activeStatRows.TryGetValue(stat.Key.ToString(), out var existingRow))
                {
                    existingRow.SetStat(stat.Key.ToString(), stat.Value);
                    existingRow.AnimateRowAppearance(count);
                }
                else
                {
                    var row = GetStatRow();
                    row.SetStat(stat.Key.ToString(), stat.Value);
                    _activeStatRows[stat.Key.ToString()] = row;
                    row.AnimateRowAppearance(count);
                }
                
                count++;
            }
        }
    }
}