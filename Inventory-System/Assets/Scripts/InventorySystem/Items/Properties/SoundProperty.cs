﻿using System;
using GoggleImporter.ItemParser.PropertySetters;
using UnityEngine;

namespace InventorySystem.Items.Properties
{
    [Serializable]
    public class SoundProperty : Property
    {
        public AudioClip Sound;
    }
}