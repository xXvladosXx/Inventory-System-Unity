using System;
using InventorySystem.Slots;
using UnityEngine;
using UnityEngine.Serialization;

namespace InventorySystem.UI
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private InventoryPanel _inventoryPanel;
        [SerializeField] private ItemContainer _itemContainer;
        [SerializeField] private int _inventorySize = 10;
        
        private void Start()
        {
            _inventoryPanel.Initialize(_inventorySize);
            
            _itemContainer.OnItemsUpdated += OnItemsUpdated;   
        }

        private void OnDestroy()
        {
            _itemContainer.OnItemsUpdated -= OnItemsUpdated;
        }

        private void OnItemsUpdated((int, IItem) obj)
        {
            _inventoryPanel.UpdateSlot(obj.Item1, obj.Item2.Icon, 1);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _inventoryPanel.Show();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _inventoryPanel.Hide();
            }
        }
    }
}