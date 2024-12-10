using GoggleImporter.Runtime.ItemParser.Types;
using GoggleImporter.Runtime.ItemParser.Types.Attribute;

namespace InventorySystem.Items.Types
{
    [ActionType]
    public class TransferableAction : ActionType
    {
        public override string ToString() => "Transfer";
    }
}