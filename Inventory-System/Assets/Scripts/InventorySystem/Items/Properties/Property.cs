using System;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public abstract class Property
    {
        public string Name;
        public bool ResetableOnImport = true;
        
        public abstract IPropertySetter PropertySetter { get; }
    }
}