using System;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class TypeParser : BaseParser
    {
        public override string PropertyType => "Type";

        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (Enum.TryParse<PropertyType>(token, out var type))
            {
                itemSettings.CurrentType = type;
            }
            else
            {
                Debug.LogError($"Failed to parse Type: {token}");
            }
        }
    }
}