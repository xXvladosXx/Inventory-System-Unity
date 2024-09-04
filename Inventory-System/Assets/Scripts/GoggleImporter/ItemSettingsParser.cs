using System.Collections.Generic;
using InventorySystem.Items.Properties;
using UnityEngine;

namespace GoggleImporter
{
    public class ItemSettingsParser : IGoogleSheetParser
    {
        private readonly GameSettings _gameSettings;
        private ItemSettings _currentItemSettings;
        private Property _currentProperty;

        public ItemSettingsParser(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            _gameSettings.Items = new List<ItemSettings>();
        }

        public void ParseSheet(List<string> headers, IList<object> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                Debug.LogWarning("No data to parse");
                return;
            }

            _currentItemSettings = new ItemSettings();

            for (int i = 0; i < headers.Count; i++)
            {
                var header = headers[i];

                if (string.IsNullOrEmpty(header))
                    continue;

                if (i >= tokens.Count)
                    continue;

                var token = tokens[i]?.ToString();

                switch (header)
                {
                    case "ItemName":
                        _currentItemSettings.Name = token;
                        break;

                    case "IsStackable":
                        _currentItemSettings.IsStackable = token?.ToLower() == "yes";
                        break;

                    case "StackSize":
                        if (int.TryParse(token, out var stackSize))
                        {
                            _currentItemSettings.MaxInStack = stackSize;
                        }
                        else
                        {
                            Debug.LogWarning($"Invalid StackSize value: {token}");
                        }

                        break;

                    case "OneValueProperty":
                        if (string.IsNullOrEmpty(token)) break;

                        var properties = token.Split(';');
                        for (int j = 0; j < properties.Length; j += 2)
                        {
                            if (j + 1 < properties.Length)
                            {
                                var propertyName = properties[j];
                                if (int.TryParse(properties[j + 1], out var propertyValue))
                                {
                                    _currentItemSettings.OneValueProperties.Add(new OneValueProperty
                                    {
                                        Name = propertyName,
                                        Value = propertyValue
                                    });
                                }
                                else
                                {
                                    Debug.LogWarning($"Invalid property value: {properties[j + 1]}");
                                }
                            }
                        }

                        break;

                    default:
                        Debug.LogWarning($"Unknown header: {header}");
                        break;
                }
            }

            _gameSettings.Items.Add(_currentItemSettings);
        }
    }
}