using System;
using Example.InventorySystem.Configs;
using Example.InventorySystem.Items;
using GoggleImporter.Runtime.ItemParser.Parsers;
using UnityEngine;

namespace Example.Parsers
{
    public class ItemTypeParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => "ItemType";
        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            if (Enum.TryParse<ItemType>(token, out var parsedType))
            {
                itemParsableSettings.ItemType = parsedType;
            }
            else
            {
                Debug.LogWarning($"Invalid ItemType token: {token}");
            }
        }
    }
}