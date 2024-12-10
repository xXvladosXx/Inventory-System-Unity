using System;
using Example.StatsSystem.Stats;
using GoggleImporter.Runtime.ItemParser.Property;
using InventorySystem.Items.Types;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class ConstantStatProperty : Property
    {
        public float Value;
        public StatType StatType;

        public override string ToString() => 
            Value > 0 ? $"{StatType}: +{Value}" : $"{StatType}: {Value}";
    }
}