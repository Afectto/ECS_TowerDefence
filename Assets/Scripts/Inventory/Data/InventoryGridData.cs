using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryGridData
{
    public string OwnerID;
    public List<InventorySlotData> SlotsData;
    public Vector2Int Size;
}
