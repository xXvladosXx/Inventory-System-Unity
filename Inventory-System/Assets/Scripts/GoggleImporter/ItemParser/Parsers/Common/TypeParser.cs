using System;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class TypeParser : BaseParser
    {
        public override Property Property { get; }
        public override string PropertyType => "Type";

        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (Enum.TryParse<ActionType>(token, out var type))
            {
                itemSettings.SetCurrentType(type);
            }
            else
            {
                Debug.LogError($"Failed to parse Type: {token}");
            }
        }
    }
}