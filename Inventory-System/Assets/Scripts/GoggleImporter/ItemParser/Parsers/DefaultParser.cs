using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers
{
    public class DefaultParser : BaseParser
    {
        public override string PropertyType => "UnknownHeader"; 

        public override void Parse(string token, ItemSettings itemSettings)
        {
            Debug.LogWarning($"Unknown header encountered: {PropertyType}");
        }
    }
}