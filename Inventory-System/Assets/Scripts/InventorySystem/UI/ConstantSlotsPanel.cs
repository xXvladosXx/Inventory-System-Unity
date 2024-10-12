namespace InventorySystem.UI
{
    public class ConstantSlotsPanel : BaseItemContainerPanel
    {
        protected override void InitializeSlots()
        {
            foreach (var slot in _slots)
            {
                AddSlotListeners(slot);
            }
        }
    }
}