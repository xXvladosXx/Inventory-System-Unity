using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GoggleImporter.ItemParser.Types;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace GoggleImporter.ItemParser
{
    [Serializable]
    public class ItemSettings
    {
        public string Name;
        public bool IsStackable;
        public int MaxInStack;
        
        public PropertyType? CurrentType { get; private set; }

        public List<TypeToConstantStat> ConstantStats = new List<TypeToConstantStat>();
        public List<TypeToEquipType> EquipTypes = new List<TypeToEquipType>();

        public void SetCurrentType(PropertyType propertyType)
        {
            CurrentType = propertyType;
        }
    }
}