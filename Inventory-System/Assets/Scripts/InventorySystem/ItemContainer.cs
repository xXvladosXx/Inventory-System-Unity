using System;
using System.Collections.Generic;
using InventorySystem.Slots;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace InventorySystem
{
    public class ItemContainer : SerializedMonoBehaviour
    {
        [field: SerializeField] public int Size { get; private set; } = 10;
        
        [SerializeField] private List<IItem> _items = new List<IItem>();
        [SerializeField] private IItem _item;
        [SerializeField] private IItem _item2;
        public event Action<(int, IItem)> OnItemsUpdated;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _items.Add(_item);
                OnItemsUpdated?.Invoke((_items.IndexOf(_item), _item));
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                _items.Add(_item2);
                OnItemsUpdated?.Invoke((_items.IndexOf(_item2), _item2));
            }
        }
    }
}
