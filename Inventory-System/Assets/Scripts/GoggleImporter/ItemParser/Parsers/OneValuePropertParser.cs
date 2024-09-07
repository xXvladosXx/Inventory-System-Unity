using System;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;

namespace GoggleImporter.ItemParser.Parsers
{
    public class OneValuePropertyParser : BaseParser, IPropertySetter
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
                        itemSettings.OneValueProperties.Add(new OneValueProperty
                        {
                            Name = propertyName,
                            Value = propertyValue
                        });
                    }
                }
            }
        }

        public void Set(Property property, Item item)
        {
            if (property is OneValueProperty oneValueProperty)
            {
                oneValueProperty.ResetableOnImport = true;
                item.Properties.Add((PropertyName)Enum.Parse(typeof(PropertyName), property.Name), oneValueProperty);
            }
        }
    }
}