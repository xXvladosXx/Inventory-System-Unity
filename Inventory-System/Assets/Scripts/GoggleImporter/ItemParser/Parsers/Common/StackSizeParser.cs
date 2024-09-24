using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class StackSizeParser : BaseParser
    {
        public override string PropertyType => "StackSize";

        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (int.TryParse(token, out var stackSize))
            {
                itemSettings.MaxInStack = stackSize;
            }
            else
            {
                Debug.LogWarning($"Invalid StackSize value: {token}");
            }
        }
    }
}