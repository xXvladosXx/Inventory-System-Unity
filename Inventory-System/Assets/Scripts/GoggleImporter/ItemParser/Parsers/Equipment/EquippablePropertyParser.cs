using System;
using System.Collections.Generic;
using GoggleImporter.ItemParser.PropertySetters;
using GoggleImporter.ItemParser.Types;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Equipment
{
    public class EquippablePropertyParser : BaseParser, IPropertySetter
    {
        public override string PropertyType => nameof(EquippableProperty);

        public override void Parse(string token, ItemSettings itemSettings)
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

            if (itemSettings.CurrentType != null)
            {
                itemSettings.AllProperties.Add(new ActionTypeToProperty
                {
                    ActionType = itemSettings.CurrentType,
                    Property = property
                });
            }
            else
            {
                Debug.LogWarning($"No type set for EquippableProperty. Default Level: {property.Level}. Item: {itemSettings.Name}");
            }
        }
    }
}