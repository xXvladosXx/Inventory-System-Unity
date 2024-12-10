using System;
using System.Collections.Generic;
using System.Linq;
using Example.InventorySystem.Configs;
using GoggleImporter.Runtime.ItemParser.Parsers;
using GoggleImporter.Runtime.ItemParser.Types;
using GoggleImporter.Runtime.ItemParser.Types.Attribute;
using UnityEngine;

namespace Example.Parsers.Common
{
    public class TypeParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => "Type";

        private readonly Dictionary<string, Type> _actionTypeMapping = new Dictionary<string, Type>();

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(ActionTypeAttribute), false).Length > 0);

            foreach (var type in types)
            {
                _actionTypeMapping[type.Name] = type;
            }
            
            if (_actionTypeMapping.TryGetValue(token, out var parsedType))
            {
                var actionTypeInstance = (ActionType)Activator.CreateInstance(parsedType);
                itemParsableSettings.SetCurrentType(actionTypeInstance);
            }
            else
            {
                Debug.LogError($"Failed to parse Type: {token}");
            }
        }
    }
}