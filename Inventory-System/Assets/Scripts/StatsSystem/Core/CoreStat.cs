using System;
using InventorySystem.Items.Stats;
using UnityEngine;

namespace StatsSystem
{
    [Serializable]
    public struct CoreStat
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private float _value;

        public StatType StatType => _statType;
        public float Value => _value;

        public CoreStat(StatType statType, float value)
        {
            _statType = statType;
            _value = value;
        }
    }
}