﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GoggleImporter.ItemParser.Types;
using InventorySystem.Items;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.PropertySetters
{
    public class PropertySettersCollector
    {
        private readonly Dictionary<string, IPropertySetter> _parsers = new Dictionary<string, IPropertySetter>();

        public PropertySettersCollector()
        {
            var parserTypes = typeof(IPropertySetter).Assembly.GetTypes()
                .Where(t => typeof(IPropertySetter).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in parserTypes)
            {
                var parser = (IPropertySetter)Activator.CreateInstance(type);
                _parsers.Add(parser.PropertyType, parser);
            }
        }
        
        public void SetProperties(ItemSettings itemSettings, Item item)
        {
            var fields = itemSettings.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (typeof(IEnumerable).IsAssignableFrom(field.FieldType) && field.FieldType.IsGenericType)
                {
                    var elementType = field.FieldType.GetGenericArguments()[0];

                    if (typeof(IPropertyWithType).IsAssignableFrom(elementType))
                    {
                        var list = (IEnumerable)field.GetValue(itemSettings);

                        if (list == null) continue;

                        foreach (var propertyWithType in list)
                        {
                            var typedProperty = (IPropertyWithType)propertyWithType;

                            if (typedProperty.Property is { } property)
                            {
                                SetProperty(typedProperty.PropertyType, property, item);
                            }
                        }
                    }
                }
            }
        }

        private void SetProperty(PropertyType type, Property property, Item item)
        {
            var propertyType = property.GetType().Name;

            if (_parsers.TryGetValue(propertyType, out var parser))
            {
                parser.Set(type, property, item);
            }
            else
            {
                Debug.LogWarning($"No parser found for property type {propertyType}");
            }
        }
    }
}