using System;
using System.Collections.Generic;
using InventorySystem.Items.Properties;
using UnityEngine.Serialization;

namespace GoggleImporter
{
    [Serializable]
    public class ItemSettings
    {
        public string Name;
        public bool IsStackable;
        public int MaxInStack;
        public List<OneValueProperty> OneValueProperties = new List<OneValueProperty>();
    }
}