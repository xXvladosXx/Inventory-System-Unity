using System;
using GoggleImporter.ItemParser.Parsers;
using GoggleImporter.ItemParser.Parsers.PropertySetters;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class ConstantStatProperty : Property
    {
        public float Value;
        public override IPropertySetter PropertySetter { get; } = new ConstantStatPropertyParser();
    }
}