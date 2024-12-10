using System;
using Example.InventorySystem.Configs;
using GoggleImporter.Runtime.ItemParser.Parsers;
using GoggleImporter.Runtime.ItemParser.Types;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace Example.Parsers.Equipment
{
    public class EquippablePropertyParser : PropertyParser<ItemParsableData>
    {
        public override string PropertyType => nameof(EquippableProperty);

        public override void Parse(string token, ItemParsableData itemParsableSettings)
        {
            if (string.IsNullOrEmpty(token)) return;

            var propertyParts = token.Split(';');
            var equipTypeValue = propertyParts[0];
            var levelValue = propertyParts.Length > 1 ? propertyParts[1] : "0";

            if (!Enum.TryParse(equipTypeValue, out EquipType equipType))
            {
                Debug.LogError($"Invalid EquipType for EquippableProperty: {equipTypeValue}");
                return;
            }

            var property = new EquippableProperty
            {
                EquipType = equipType,
                Level = int.TryParse(levelValue, out int level) ? level : 0
            };

            if (itemParsableSettings.CurrentType != null)
            {
                itemParsableSettings.AllProperties.Add(new ActionTypeToProperty
                {
                    ActionType = itemParsableSettings.CurrentType,
                    Property = property
                });
            }
            else
            {
                Debug.LogWarning($"No type set for EquippableProperty. Default Level: {property.Level}. Item: {itemParsableSettings.Name}");
            }
        }
    }
}