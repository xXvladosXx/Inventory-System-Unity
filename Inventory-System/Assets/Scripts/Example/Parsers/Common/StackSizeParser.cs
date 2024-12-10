using Example.InventorySystem.Configs;
using GoggleImporter.Runtime.ItemParser.Parsers;
using UnityEngine;

namespace Example.Parsers.Common
{
    public class StackSizeParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => "StackSize";

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            if (int.TryParse(token, out var stackSize))
            {
                itemParsableSettings.MaxInStack = stackSize;
            }
            else
            {
                Debug.LogWarning($"Invalid StackSize value: {token}");
            }
        }
    }
}