using System;
using UnityEngine;

namespace InventorySystem.Slots
{
    [Serializable]
    public class Slot : ISlot
    {
        [field: SerializeField] public IItem CurrentItem { get; private set; }
        [field: SerializeField] public int CurrentAmount { get; private set; }
    }
}