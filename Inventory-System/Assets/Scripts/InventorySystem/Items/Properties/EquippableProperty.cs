using GoggleImporter.ItemParser.Parsers.Equipment;
using GoggleImporter.ItemParser.PropertySetters;
using InventorySystem.Items.Types;

namespace InventorySystem.Items.Properties
{
    public class EquippableProperty : Property
    {
        public EquipType EquipType;
        public override IPropertySetter PropertySetter { get; } = new EquippablePropertyParser();
    }
}