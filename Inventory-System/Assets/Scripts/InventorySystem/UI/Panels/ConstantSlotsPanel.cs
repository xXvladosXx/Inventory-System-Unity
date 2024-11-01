using System.Linq;
using InventorySystem.UI.Slots;
using Sirenix.OdinInspector;

namespace InventorySystem.UI.Panels
{
    public class ConstantSlotsPanel : BaseItemContainerPanel
    {
        protected override void InitializeSlots()
        {
            foreach (var slot in slots)
            {
                AddSlotListeners(slot);
            }
        }

        [Button]
        private void FindSlots()
        {
            slots = GetComponentsInChildren<ContainerSlot>().ToList();
        }
    }
}