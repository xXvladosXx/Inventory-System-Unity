using System;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Types
{
    [Serializable]
    public class TypeToEquip : IPropertyWithType
    {
        [field: SerializeField] public ActionType ActionType { get; set; }
        [field: SerializeField] public EquippableProperty EquipProperty { get; set; }
        public Property Property => EquipProperty;
    }
}