using System;
using System.Collections.Generic;
using UnityEngine;

namespace GoggleImporter.PropertyParser
{
    public class PropertyNameParser : IGoogleSheetParser
    {
        private readonly GameSettings _gameSettings;

        public PropertyNameParser(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public void ParseSheet(List<string> headers, IList<object> tokens)
        {
            if (tokens == null || tokens.Count == 0)
            {
                Debug.LogWarning("No data to parse for PropertyName");
                return;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                var propertyNameToken = tokens[i]?.ToString();
                if (string.IsNullOrEmpty(propertyNameToken))
                {
                    Debug.LogWarning("Empty PropertyName value, skipping.");
                    continue;
                }
                
                _gameSettings.PropertyNames.Add(propertyNameToken);
            }
        }
    }
}