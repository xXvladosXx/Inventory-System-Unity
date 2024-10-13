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
        
        public ActionType? CurrentType { get; private set; }

        public List<ActionTypeToConstantStatProperty> ConstantStatsProperties = new List<ActionTypeToConstantStatProperty>();
        public List<ActionTypeToEquipProperty> EquipProperties = new List<ActionTypeToEquipProperty>();

        public void SetCurrentType(ActionType actionType)
        {
            CurrentType = actionType;
        }
    }
}