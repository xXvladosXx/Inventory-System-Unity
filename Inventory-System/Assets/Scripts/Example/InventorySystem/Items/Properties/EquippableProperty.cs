using GoggleImporter.Runtime.ItemParser.Property;
using InventorySystem.Items.Types;

namespace InventorySystem.Items.Properties
{
    public class EquippableProperty : Property
    {
        public EquipType EquipType;
        public int Level;
        public override string ToString() => string.Empty;
    }
}