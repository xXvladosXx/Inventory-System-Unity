using System;
using Example.InventorySystem.Configs;
using Example.StatsSystem.Stats;
using GoggleImporter.Runtime.ItemParser.Parsers;
using GoggleImporter.Runtime.ItemParser.Types;
using InventorySystem.Items.Properties;
using UnityEngine;

namespace Example.Parsers.Stats
{
    public class ConstantStatPropertyParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => nameof(ConstantStatProperty);

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            if (string.IsNullOrEmpty(token)) return;

            var propertyParts = token.Split(';');
            if (propertyParts.Length < 2) 
            {
                Debug.LogError($"Invalid format for property: {token}");
                return;
            }

            var propertyTypeName = propertyParts[0];
            var propertyValue = propertyParts[1];   

            if (int.TryParse(propertyValue, out var value))
            {
                var property = new ConstantStatProperty
                {
                    StatType =  (StatType)Enum.Parse(typeof(StatType), propertyTypeName),
                    Value = value
                };

                if (itemParsableSettings.CurrentType != null)
                {
                    itemParsableSettings.AllProperties.Add(new ActionTypeToProperty()
                    {
                        ActionType = itemParsableSettings.CurrentType,
                        Property = property
                    });
                }
                else
                {
                    Debug.LogError("No type set for property. Item " + itemParsableSettings.Name);
                }
            }
            else
            {
                Debug.LogError($"Invalid value for property: {token}");
            }
        }
    }
}