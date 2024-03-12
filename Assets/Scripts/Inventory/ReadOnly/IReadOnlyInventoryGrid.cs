
using System;
using UnityEngine;

public interface IReadOnlyInventoryGrid : IReadOnlyInventory
{
    event Action<Vector2Int> SizeChange;
    
    Vector2Int Size { get; }

    IReadOnlyInventorySlot<Sprite, int>[,] GetSlots();
}