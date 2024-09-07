using InventorySystem.Items.Properties;
using InventorySystem.Slots;

namespace GoggleImporter.ItemParser.Parsers.PropertySetters
{
    public interface IPropertySetter
    {
        string PropertyType { get; }  
        void Set(Property property, Item item);
    }
}