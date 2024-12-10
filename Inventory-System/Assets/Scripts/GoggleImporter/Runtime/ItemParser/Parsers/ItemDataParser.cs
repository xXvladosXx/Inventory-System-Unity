using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GoggleImporter.Runtime.ItemParser.Item;
using GoggleImporter.Runtime.ItemParser.Property;
using GoggleImporter.Runtime.ItemParser.Types;
using UnityEngine;

namespace GoggleImporter.Runtime.ItemParser.Parsers
{
    public class ItemDataParser<T> : IGoogleSheetParser where T : IItemParsableData, new()
    {
        private readonly List<T> _items;
        private T _currentItemSettings;

        private readonly Dictionary<string, PropertyParser<T>> _parsers;

        public ItemDataParser(List<T> items)
        {
            _items = items;
            _items.Clear();

            _parsers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => typeof(PropertyParser<T>).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (PropertyParser<T>)Activator.CreateInstance(t))
                .ToDictionary(p => p.PropertyType, p => p);
        }

        public void ParseSheet(List<string> headers, IList<object> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                Debug.LogWarning("No data to parse");
                return;
            }

            bool isEmptyRow = tokens.All(token => string.IsNullOrEmpty(token?.ToString()));
            if (isEmptyRow)
            {
                Debug.Log("Empty row found, skipping.");
                return;
            }
         
            _currentItemSettings = new T(); 

            for (int i = 0; i < headers.Count; i++)
            {
                var header = headers[i];
                var token = i < tokens.Count ? tokens[i]?.ToString() : null;

                if (string.IsNullOrEmpty(header) || string.IsNullOrEmpty(token))
                    continue;

                var parser = GetParserForHeader(header);
                if (parser != null)
                {
                    parser.Parse(token, _currentItemSettings);
                }
                else
                {
                    Debug.LogWarning($"Unknown header: {header}");
                }
            }

            _items.Add(_currentItemSettings);
        }
        
        public void SetProperties(IItemParsableData itemParsableSettings, IParsableItem item)
        {
            var fields = itemParsableSettings.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (typeof(IEnumerable).IsAssignableFrom(field.FieldType) && field.FieldType.IsGenericType)
                {
                    var elementType = field.FieldType.GetGenericArguments()[0];

                    if (typeof(ActionTypeToProperty).IsAssignableFrom(elementType))
                    {
                        var list = (IEnumerable)field.GetValue(itemParsableSettings);

                        if (list == null) continue;

                        foreach (var propertyWithType in list)
                        {
                            var typedProperty = (ActionTypeToProperty)propertyWithType;

                            if (typedProperty.Property is { } property)
                            {
                                SetProperty(typedProperty.ActionType, property, item);
                            }
                        }
                    }
                }
            }
        }

        private void SetProperty(ActionType type, Property.Property property, IParsableItem item)
        {
            var propertyType = property.GetType().Name;

            if (_parsers.TryGetValue(propertyType, out var parser))
            {
                AddProperty(type, property, item);
            }
            else
            {
                Debug.LogWarning($"No parser found for property type {propertyType}");
            }
        }
        
        void AddProperty<TProperty>(ActionType actionType, TProperty property, IParsableItem item)
            where TProperty : Property.Property
        {
            if (!item.Properties.TryGetValue(actionType, out var propertiesList))
            {
                propertiesList = new List<Property.Property>();
                item.Properties[actionType] = propertiesList;
            }

            propertiesList.Add(property);
        }

        private PropertyParser<T> GetParserForHeader(string header) => 
            _parsers.FirstOrDefault(p => header.StartsWith(p.Key)).Value;
    }
}