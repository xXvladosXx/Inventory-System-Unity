using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;

namespace GoggleImporter.ItemParser.Types
{
    public interface IActionTypeToProperty
    {
        ActionType ActionType { get; }
        Property Property { get; }
    }
}