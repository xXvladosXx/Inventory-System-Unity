using GoggleImporter.Runtime.ItemParser.Item;

namespace GoggleImporter.Runtime.ItemParser.Parsers
{
    public abstract class PropertyParser<T> where T : IItemParsableData
    {
        public abstract string PropertyType { get; }
        public abstract void Parse(string token, T itemSettings);
    }
}