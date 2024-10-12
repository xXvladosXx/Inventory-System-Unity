using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;

namespace GoggleImporter.ItemParser.Types
{
    public interface IPropertyWithType
    {
        ActionType ActionType { get; }
        Property Property { get; }
    }
}