using UnityEngine;

namespace InventorySystem.UI
{
    public class DynamicSlotsPanel : BaseItemContainerPanel
    {
        [SerializeField] protected ContainerSlot _containerSlotPrefab;

        protected override void InitializeSlots()
        {
            for (int i = 0; i < ItemContainer.Size; i++)
            {
                var slot = Instantiate(_containerSlotPrefab, _content);
                _slots.Add(slot);
                AddSlotListeners(slot);
            }
        }
    }
}