using System;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class ItemTypeParser : BaseParser
    {
        public override Property Property { get; }
        public override string PropertyType => "ItemType";
        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (Enum.TryParse<ItemType>(token, out var parsedType))
            {
                itemSettings.ItemType = parsedType;
            }
            else
            {
                Debug.LogWarning($"Invalid ItemType token: {token}");
            }
        }
    }
}