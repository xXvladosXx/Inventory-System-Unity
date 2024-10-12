using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace InventorySystem.Loot
{
    public class LootOpener : MonoBehaviour
    {
        [SerializeField] private List<ItemContainer> _lootContainers = new List<ItemContainer>();
        public event Action<ItemContainer> OnLootContainerOpened;

        [Button]
        public void OpenLootContainer(int index)
        {
            OnLootContainerOpened?.Invoke(_lootContainers[index]);
        }
    }
}