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
        public override PropertyType PropertyType => PropertyType.ConstantStatProperty;
        public float Value;
        public StatType StatType;
    }
}