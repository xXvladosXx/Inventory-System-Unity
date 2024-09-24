using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GoggleImporter.ItemParser.Parsers;
using UnityEngine;

namespace GoggleImporter.ItemParser
{
    public class ItemSettingsParser : IGoogleSheetParser
    {
        private readonly GameSettings _gameSettings;
        private ItemSettings _currentItemSettings;

        private Dictionary<string, BaseParser> _parsers;

        public ItemSettingsParser(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _gameSettings.Items = new List<ItemSettings>();

            _parsers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(BaseParser).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => (BaseParser) Activator.CreateInstance(t))
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

            _currentItemSettings = new ItemSettings();

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

            _gameSettings.Items.Add(_currentItemSettings);
        }

        private BaseParser GetParserForHeader(string header) => 
            _parsers.FirstOrDefault(p => header.StartsWith(p.Key)).Value;
    }
}