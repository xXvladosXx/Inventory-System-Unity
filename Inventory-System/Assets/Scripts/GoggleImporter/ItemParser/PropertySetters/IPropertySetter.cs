using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;

namespace GoggleImporter.ItemParser.PropertySetters
{
    public interface IPropertySetter
    {
        string PropertyType { get; }  
        void Set(ActionType actionType, Property property, Item item);
    }
}