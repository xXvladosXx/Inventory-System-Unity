using InventorySystem.Items.Properties;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class ItemNameParser : BaseParser
    {
        public override string PropertyType => "ItemName";

        public override void Parse(string token, ItemSettings itemSettings)
        {
            itemSettings.Name = token;
        }
    }
}