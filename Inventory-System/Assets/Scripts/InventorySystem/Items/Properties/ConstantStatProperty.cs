using System;
using GoggleImporter.ItemParser.Parsers.Stats;
using GoggleImporter.ItemParser.PropertySetters;
using InventorySystem.Items.Stats;
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