using System;
using GoggleImporter.ItemParser.Parsers;
using GoggleImporter.ItemParser.Parsers.PropertySetters;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class DoubleValueProperty : Property
    {
        public float Value1;
        public float Value2;
        public override IPropertySetter PropertySetter { get; } = new DoubleValuePropertyParser();
    }
}