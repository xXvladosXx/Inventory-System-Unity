using System;
using InventorySystem.Items.Properties;
using InventorySystem.Items.Types;
using UnityEngine;

namespace GoggleImporter.ItemParser.Types
{
    [Serializable]
    public class TypeToConstantStat : IPropertyWithType
    {
        [field: SerializeField] public PropertyType PropertyType { get;  set; }
        [field: SerializeField] public ConstantStatProperty ConstantStat { get;  set; }
        
        public Property Property => ConstantStat;
    }
    
    [Serializable]
    public class TypeToEquipType : IPropertyWithType
    {
        [field: SerializeField] public PropertyType PropertyType { get; set; }
        [field: SerializeField] public EquippableProperty EquipProperty { get; set; }
        public Property Property => EquipProperty;
    }
}