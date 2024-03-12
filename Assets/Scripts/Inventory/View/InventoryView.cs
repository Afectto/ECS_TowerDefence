using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventorySlotView[] _slots;

    public InventorySlotView[] slots 
    { 
        get => _slots;
        private set => _slots = value;
    }

    public InventorySlotView GetInventorySlotView(int index)
    {
        return slots[index];
    }
}