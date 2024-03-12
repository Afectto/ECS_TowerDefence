using System.Collections.Generic;
using UnityEngine;

public class InventoryService
{
    private readonly Dictionary<string, InventoryGrid> _inventoriesMap = new Dictionary<string, InventoryGrid>();

    public InventoryGrid RegisterInventory(InventoryGridData inventoryGridData)
    {
        var inventory = new InventoryGrid(inventoryGridData);
        _inventoriesMap[inventory.OwnerId] = inventory;
        return inventory;
    }

    public void AddToFirstAvailableSlot(string ownerId, string itemsId, int price, int amount = 1)
    {
        var inventory = _inventoriesMap[ownerId];
        inventory.AddToFirstAvailableSlot(itemsId, price, amount);
    }
    
    public AddItemsToInventoryGridResult AddItemsToInventory(string ownerId, string itemsId, int amount = 1)
    {
        var inventory = _inventoriesMap[ownerId];
        return inventory.AddItem(itemsId, amount);
    }
    
    public AddItemsToInventoryGridResult AddItemsToInventory(string ownerId, Vector2Int slotCoords, string itemsId, int amount = 1)
    {
        var inventory = _inventoriesMap[ownerId];
        return inventory.AddItem(slotCoords, itemsId, amount);
    }

    public RemoveItemsFromInventoryGridResult RemoveItems(string ownerId, string itemsId, int amount = 1)
    {
        var inventory = _inventoriesMap[ownerId];
        return inventory.RemoveItems(itemsId, amount);
    }

    public RemoveItemsFromInventoryGridResult RemoveItems(string ownerId, Vector2Int slotCoords, string itemsId,
        int amount = 1)
    {
        var inventory = _inventoriesMap[ownerId];
        return inventory.RemoveItems(slotCoords, itemsId, amount);
    }

    public void CleanInventory(string ownerId)
    {
        var inventory = _inventoriesMap[ownerId];
        inventory.CleanInventory();
    }

    public bool Has(string ownerId, string itemId, int amount)
    {
        var inventory = _inventoriesMap[ownerId];
        return inventory.Has(itemId, amount);
    }

    public IReadOnlyInventoryGrid GetInventory(string ownerId)
    {
        return _inventoriesMap[ownerId];
    }

}
