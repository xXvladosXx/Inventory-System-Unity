using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;

namespace GoggleImporter.ItemParser
{
    [Serializable]
    public class ItemSettings
    {
        public string Name;
        public bool IsStackable;
        public int MaxInStack;
        public List<ConstantStatProperty> OneValueProperties = new List<ConstantStatProperty>();
        public List<ConstantStatRangeProperty> DoubleValueProperties = new List<ConstantStatRangeProperty>();
    }

    public static class ItemSettingsHelper
    {
        public static void SetProperties(PropertySetter propertySetter, ItemSettings itemSettings, Item item)
        {
            var fields = itemSettings.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (typeof(IEnumerable).IsAssignableFrom(field.FieldType) && field.FieldType.IsGenericType)
                {
                    var elementType = field.FieldType.GetGenericArguments()[0];

                    if (typeof(Property).IsAssignableFrom(elementType))
                    {
                        var list = (IEnumerable)field.GetValue(itemSettings);

                        if (list == null) continue;

                        foreach (var property in list)
                        {
                            propertySetter.SetProperty((Property)property, item);
                        }
                    }
                }
            }
        }
    }
}