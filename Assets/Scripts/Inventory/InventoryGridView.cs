using UnityEngine;

public class InventoryGridView : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    public void Setup(IReadOnlyInventoryGrid inventoryGrid)
    {
        var slots = inventoryGrid.GetSlots();
        var size = inventoryGrid.Size;
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var slot = slots[i, j];
            }
        }
    }
}
