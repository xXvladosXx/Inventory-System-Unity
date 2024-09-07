using System;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;
using UnityEngine;
using PropertyName = InventorySystem.Items.Properties.PropertyName;

namespace GoggleImporter.ItemParser.Parsers
{
    public class DoubleValuePropertyParser : BaseParser, IPropertySetter
    {
        public override string PropertyType { get; } = "DoubleValueProperty";
        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (string.IsNullOrEmpty(token)) return;

            var properties = token.Split(';');
            for (int j = 0; j < properties.Length; j += 3)
            {
                if (j + 2 < properties.Length)
                {
                    var propertyName = properties[j];
                    if (int.TryParse(properties[j + 1], out var firstValue) && 
                        int.TryParse(properties[j + 2], out var secondValue))
                    {
                        itemSettings.DoubleValueProperties.Add(new DoubleValueProperty
                        {
                            Name = propertyName,
                            Value1 = firstValue,
                            Value2 = secondValue
                        });
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid property values for {propertyName}");
                    }
                }
            }
        }

        public void Set(Property property, Item item)
        {
            if (property is DoubleValueProperty doubleValueProperty)
            {
                doubleValueProperty.ResetableOnImport = true;
                item.Properties.Add((PropertyName)Enum.Parse(typeof(PropertyName), property.Name), doubleValueProperty);
            }
        }
    }
}