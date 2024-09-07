using System;
using GoggleImporter.ItemParser.Parsers.PropertySetters;
using UnityEngine;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class SoundProperty : Property
    {
        public AudioClip Sound;
        public override IPropertySetter PropertySetter { get; }
    }
}