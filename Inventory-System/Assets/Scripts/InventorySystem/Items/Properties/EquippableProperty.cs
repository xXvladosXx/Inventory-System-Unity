using GoggleImporter.ItemParser.Parsers.Equipment;
using GoggleImporter.ItemParser.PropertySetters;
using InventorySystem.Items.Types;

namespace InventorySystem.Items.Properties
{
    public class EquippableProperty : Property
    {
        public override PropertyType PropertyType => PropertyType.EquippableProperty;
        public EquipType EquipType;
        public int Level;
        public override string ToString()
        {
            var text = "";
            if (Level != 0)
            {
                text += $"Requires level: {Level} \n";
            }

            text += $"Type: {EquipType}";
            
            return text;
        }
    }
}