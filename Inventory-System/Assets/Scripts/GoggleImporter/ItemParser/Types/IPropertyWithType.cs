using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;

namespace GoggleImporter.ItemParser.Types
{
    public interface IPropertyWithType
    {
        PropertyType PropertyType { get; }
        Property Property { get; }
    }
}