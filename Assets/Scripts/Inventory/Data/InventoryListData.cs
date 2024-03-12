using System;
using System.Collections.Generic;

[Serializable]
public class InventoryListData
{
    public string OwnerID; 
    public List<InventorySlotData> SlotsData;
    public int Size;
}
