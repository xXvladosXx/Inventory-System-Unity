using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GoggleImporter.ItemParser.Types;
using InventorySystem.Items;
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
        public ItemType ItemType;

        public ActionType CurrentType { get; private set; }

        public List<IActionTypeToProperty> AllProperties = new List<IActionTypeToProperty>();

        public void SetCurrentType(ActionType actionType)
        {
            CurrentType = actionType;
        }
    }
}