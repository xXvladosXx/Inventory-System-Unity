namespace InventorySystem.Slots
{
    public interface ISlot
    {
        public IItem CurrentItem { get; }
        public int CurrentAmount { get; }
        public bool IsEmpty => CurrentItem == null;
        public bool IsFull => CurrentItem != null && CurrentAmount == CurrentItem.MaxInStack;
    }
}