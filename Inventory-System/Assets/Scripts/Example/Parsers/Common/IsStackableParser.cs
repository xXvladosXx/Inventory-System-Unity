using Example.InventorySystem.Configs;
using GoggleImporter.Runtime.ItemParser.Parsers;

namespace Example.Parsers.Common
{
    public class IsStackableParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => "IsStackable";

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            itemParsableSettings.IsStackable = token?.ToLower() == "yes";
        }
    }
}