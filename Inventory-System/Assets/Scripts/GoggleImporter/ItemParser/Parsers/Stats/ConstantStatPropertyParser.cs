using System;
using System.Collections.Generic;
using GoggleImporter.ItemParser.PropertySetters;
using GoggleImporter.ItemParser.Types;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Stats;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.Stats
{
    public class ConstantStatPropertyParser : BaseParser, IPropertySetter
    {
        public override string PropertyType => InventorySystem.Items.Types.PropertyType.ConstantStatProperty.ToString();

        public override void Parse(string token, ItemSettings itemSettings)
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

                if (itemSettings.CurrentType.HasValue)
                {
                    itemSettings.ConstantStats.Add(new TypeToConstantStat
                    {
                        ActionType = itemSettings.CurrentType.Value,
                        ConstantStat = property
                    });
                }
                else
                {
                    Debug.LogError("No type set for property. Item " + itemSettings.Name);
                }
            }
            else
            {
                Debug.LogError($"Invalid value for property: {token}");
            }
        }

        public void Set(ActionType actionType, Property property, Item item)
        {
            if (property is ConstantStatProperty constantStatProperty)
            {
                constantStatProperty.ResetableOnImport = true;

                if (!item.Properties.TryGetValue(actionType, out var propertiesList))
                {
                    propertiesList = new List<Property>();
                    item.Properties[actionType] = propertiesList;
                }

                propertiesList.Add(constantStatProperty);
            }
        }
    }
}