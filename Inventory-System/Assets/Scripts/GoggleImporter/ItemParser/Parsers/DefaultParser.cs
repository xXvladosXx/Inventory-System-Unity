using InventorySystem.Items.Properties;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers
{
    public class DefaultParser : BaseParser
    {
        public override Property Property { get; }
        public override string PropertyType => "UnknownHeader"; 

        public override void Parse(string token, ItemSettings itemSettings)
        {
            Debug.LogWarning($"Unknown header encountered: {PropertyType}");
        }
    }
}