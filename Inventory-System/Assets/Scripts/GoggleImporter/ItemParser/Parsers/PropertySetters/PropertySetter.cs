using System;
using System.Collections.Generic;
using System.Linq;
using InventorySystem.Items.Properties;
using InventorySystem.Slots;
using UnityEngine;

namespace GoggleImporter.ItemParser.Parsers.PropertySetters
{
    public class PropertySetter
    {
        private readonly Dictionary<string, IPropertySetter> _parsers = new Dictionary<string, IPropertySetter>();

        public PropertySetter()
        {
            var parserTypes = typeof(IPropertySetter).Assembly.GetTypes()
                .Where(t => typeof(IPropertySetter).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in parserTypes)
            {
                var parser = (IPropertySetter)Activator.CreateInstance(type);
                _parsers.Add(parser.PropertyType, parser);
            }
        }

        public void SetProperty(Property property, Item item)
        {
            var propertyType = property.GetType().Name;

            if (_parsers.TryGetValue(propertyType, out var parser))
            {
                parser.Set(property, item);
            }
            else
            {
                Debug.LogWarning($"No parser found for property type {propertyType}");
            }
        }
    }
}