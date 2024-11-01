using InventorySystem.UI.Slots;
using UnityEngine;

namespace InventorySystem.UI.Panels
{
    public class DynamicSlotsPanel : BaseItemContainerPanel
    {
        [SerializeField] protected ContainerSlot _containerSlotPrefab;
        [SerializeField] protected RectTransform _content;

        protected override void InitializeSlots()
        {
            for (int i = 0; i < ItemContainer.Size; i++)
            {
                var slot = Instantiate(_containerSlotPrefab, _content);
                slots.Add(slot);
                AddSlotListeners(slot);
            }
        }
    }
}