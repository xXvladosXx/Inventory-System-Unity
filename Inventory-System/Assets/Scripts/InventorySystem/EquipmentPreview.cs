using System.Collections.Generic;
using InventorySystem.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem
{
    public class EquipmentPreview : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<string, GameObject> _equipment;
        [SerializeField] private Transform _content;

        [Button]
        public void CreateEquipmentParts()
        {
            foreach (Transform child in _content)
            {
                _equipment.Add(child.name, child.gameObject);
            }
        }

        public void RefreshItems(Dictionary<int, InventoryItem> equippedItems)
        {
            DeactivateModels();

            foreach (var value in equippedItems.Values)
            {
                if (_equipment.TryGetValue(value.Item.Name, out var model))
                {
                    model.SetActive(true);
                }
            }
        }

        public void DeactivateModels()
        {
            foreach (var model in _equipment.Values)
            {
                model.SetActive(false);
            }
        }
    }
}