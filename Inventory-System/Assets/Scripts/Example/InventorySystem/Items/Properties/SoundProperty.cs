using System;
using GoggleImporter.Runtime.ItemParser.Property;
using UnityEngine;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class SoundProperty : Property
    {
        public AudioClip Sound;
    }
}