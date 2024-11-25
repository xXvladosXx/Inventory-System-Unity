using InventorySystem.UI.Slots;
using TMPro;
using UnityEngine;

namespace InventorySystem.UI.Panels
{
    public class DynamicSlotsPanel : BaseItemContainerPanel
    {
        [SerializeField] protected ContainerSlot _containerSlotPrefab;
        [SerializeField] protected RectTransform _content;
        
        [SerializeField] private TMP_InputField _searchItem;
        [SerializeField] private TMP_Dropdown _typeItem;
        
        protected override void InitializeSlots(int size)
        {
            for (int i = 0; i < size; i++)
            {
                var slot = Instantiate(_containerSlotPrefab, _content);
                slots.Add(slot);
                AddSlotListeners(slot);
            }
        }

        protected override void InitializeFilters()
        {
            if (_typeItem == null)
                return;
            
            _typeItem.onValueChanged.AddListener(OnOnItemTypeRequested);
            _searchItem.onValueChanged.AddListener(OnOnItemSearchRequested);
        }
        
        protected override void DisposeFilters()
        {
            if (_typeItem == null)
                return;

            _typeItem.onValueChanged.RemoveListener(OnOnItemTypeRequested);
            _searchItem.onValueChanged.RemoveListener(OnOnItemSearchRequested);
        }
    }
}