using InventorySystem.Items.Properties;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class IsStackableParser : BaseParser
    {
        public override string PropertyType => "IsStackable";

        public override void Parse(string token, ItemSettings itemSettings)
        {
            itemSettings.IsStackable = token?.ToLower() == "yes";
        }
    }
}