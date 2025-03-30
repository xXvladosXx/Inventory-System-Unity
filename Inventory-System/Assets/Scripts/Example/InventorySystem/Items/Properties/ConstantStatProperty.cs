using System;
using Example.StatsSystem.Stats;
using GoggleImporter.Runtime.ItemParser.Property;
using InventorySystem.Items.Types;
using InventorySystem.UI;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class ConstantStatProperty : Property
    {
        public float Value;
        public StatType StatType;

        public override string ToString()
        {
            var color = Value > 0 ? Constants.AVAILABLE_COLOR : Constants.UNAVAILABLE_COLOR;
            var sign = Value > 0 ? "+" : "";

            return $"<color={color}>{StatType}: {sign}{Value}</color>";
        }
    }
}