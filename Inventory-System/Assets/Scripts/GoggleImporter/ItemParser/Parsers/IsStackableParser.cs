namespace GoggleImporter.ItemParser.Parsers
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