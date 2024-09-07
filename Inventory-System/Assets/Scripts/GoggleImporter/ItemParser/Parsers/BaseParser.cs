namespace GoggleImporter.ItemParser.Parsers
{
    public abstract class BaseParser
    {
        public abstract string PropertyType { get; }
        public abstract void Parse(string token, ItemSettings itemSettings);
    }
}