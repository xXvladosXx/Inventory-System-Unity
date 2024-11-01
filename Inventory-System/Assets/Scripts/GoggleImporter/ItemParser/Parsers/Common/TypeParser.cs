using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Common
{
    public class TypeParser : BaseParser
    {
        public override Property Property { get; }
        public override string PropertyType => "Type";

        private readonly Dictionary<string, Type> ActionTypeMapping = new Dictionary<string, Type>();

        public override void Parse(string token, ItemSettings itemSettings)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(ActionTypeAttribute), false).Length > 0);

            foreach (var type in types)
            {
                ActionTypeMapping[type.Name] = type;
            }
            
            if (ActionTypeMapping.TryGetValue(token, out var parsedType))
            {
                var actionTypeInstance = (ActionType)Activator.CreateInstance(parsedType);
                itemSettings.SetCurrentType(actionTypeInstance);
            }
            else
            {
                Debug.LogError($"Failed to parse Type: {token}");
            }
        }
    }
}