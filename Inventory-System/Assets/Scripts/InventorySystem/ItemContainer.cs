using System;
using System.Collections.Generic;
using InventorySystem.Slots;
using Unity.VisualScripting;
using UnityEngine;

namespace InventorySystem
{
    public class ItemContainer : MonoBehaviour
    {
        [field: SerializeField] public int Size { get; private set; } = 10;
        
        [SerializeField] private List<IItem> _items = new List<IItem>();

        public event Action<Dictionary<int, IItem>> OnItemsUpdated; 
    }
}
