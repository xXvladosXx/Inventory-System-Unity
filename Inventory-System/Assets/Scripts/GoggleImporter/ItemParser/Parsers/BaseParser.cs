using InventorySystem.Items.Properties;

namespace GoggleImporter.ItemParser.Parsers
{
    public abstract class BaseParser
    {
        public abstract Property Property { get; }
        public abstract string PropertyType { get; }
        public abstract void Parse(string token, ItemSettings itemSettings);
    }
}