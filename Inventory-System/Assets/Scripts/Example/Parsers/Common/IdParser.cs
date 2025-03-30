using Example.InventorySystem.Configs;
using GoggleImporter.Runtime.ItemParser.Parsers;

namespace Example.Parsers.Common
{
    public class IdParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => "Id";

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            itemParsableSettings.Id = int.Parse(token);
        }
    }
}