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
        public override string PropertyType => InventorySystem.Items.Types.PropertyType.EquippableProperty.ToString();

        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (string.IsNullOrEmpty(token)) return;

            var propertyParts = token.Split(';');
            if (propertyParts.Length < 1)
            {
                Debug.LogError($"Invalid format for EquippableProperty: {token}");
                return;
            }

            var propertyValue = propertyParts[0]; 

            var property = new EquippableProperty
            {
                EquipType = (EquipType)Enum.Parse(typeof(EquipType), propertyValue)
            };

            if (itemSettings.CurrentType.HasValue)
            {
                itemSettings.EquipTypes.Add(new TypeToEquip
                {
                    ActionType = itemSettings.CurrentType.Value,
                    EquipProperty = property
                });
            }
            else
            {
                Debug.LogError("No type set for EquippableProperty. Item " + itemSettings.Name);
            }
        }

        public void Set(ActionType actionType, Property property, Item item)
        {
            if (property is EquippableProperty equippableProperty)
            {
                if (!item.Properties.TryGetValue(actionType, out var propertiesList))
                {
                    propertiesList = new List<Property>();
                    item.Properties[actionType] = propertiesList;
                }

                propertiesList.Add(equippableProperty);
            }
        }
    }
}