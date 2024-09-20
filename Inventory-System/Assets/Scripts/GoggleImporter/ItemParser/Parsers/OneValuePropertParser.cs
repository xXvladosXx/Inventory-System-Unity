using System;
using System.Collections.Generic;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;

namespace GoggleImporter.ItemParser.Parsers
{
    public class ConstantStatPropertyParser : BaseParser, IPropertySetter
    {
        public override string PropertyType => "OneValueProperty";

        public override void Parse(string token, ItemSettings itemSettings)
        {
            if (string.IsNullOrEmpty(token)) return;

            var properties = token.Split(';');
            for (int j = 0; j < properties.Length; j += 2)
            {
                if (j + 1 < properties.Length)
                {
                    var propertyName = properties[j];
                    if (int.TryParse(properties[j + 1], out var propertyValue))
                    {
                        var oneValueProperty = new ConstantStatProperty
                        {
                            Name = propertyName,
                            Value = propertyValue
                        };
                    
                        itemSettings.OneValueProperties.Add(oneValueProperty);
                    }
                }
            }
        }

        public void Set(Property property, Item item)
        {
            if (property is ConstantStatProperty oneValueProperty)
            {
                oneValueProperty.ResetableOnImport = true;

                if (!item.Properties.TryGetValue((PropertyName)Enum.Parse(typeof(PropertyName), property.Name), out var propertiesList))
                {
                    propertiesList = new List<Property>();
                    item.Properties[(PropertyName)Enum.Parse(typeof(PropertyName), property.Name)] = propertiesList;
                }

                propertiesList.Add(oneValueProperty);
            }
        }
    }
}