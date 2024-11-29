using System;
using GoggleImporter.ItemParser.PropertySetters;
using InventorySystem.Items.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public abstract class Property
    {
        public bool ResetableOnImport = true;
    }
}