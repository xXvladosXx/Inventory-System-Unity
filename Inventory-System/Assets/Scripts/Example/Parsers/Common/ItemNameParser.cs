using Example.InventorySystem.Configs;
using GoggleImporter.Runtime.ItemParser.Parsers;

namespace Example.Parsers.Common
{
    public class ItemNameParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => "ItemName";

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            itemParsableSettings.Name = token;
        }
    }
}