using System;
using GoggleImporter.ItemParser.Parsers.Stats;
using GoggleImporter.ItemParser.PropertySetters;
using InventorySystem.Items.Stats;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class ConstantStatProperty : Property
    {
        public float Value;
        public StatType StatType;
        public override IPropertySetter PropertySetter { get; } = new ConstantStatPropertyParser();
    }
}