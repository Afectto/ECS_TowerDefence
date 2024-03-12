using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGrid : IReadOnlyInventoryGrid
{
    public event Action<string, int> ItemAdded;
    public event Action<string, int> ItemRemoved;
    public event Action<Vector2Int> SizeChange;

    public string OwnerId => _data.OwnerID;
    
    public Vector2Int Size {        
        get => _data.Size;
        set
        {
            if (_data.Size != value)
            {
                _data.Size = value;
                SizeChange?.Invoke(value);
            }
        }}

    private readonly InventoryGridData _data;
    private readonly Dictionary<Vector2Int, InventorySlot> _slotsMap = new Dictionary<Vector2Int, InventorySlot>();
    
    public InventoryGrid(InventoryGridData data)
    {
        _data = data;

        var size = _data.Size;
        var lenght = Math.Max(size.x, size.y);
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                var index = i * lenght + j;
                var slotData = data.SlotsData[index];
                var slot = new InventorySlot(slotData);
                var position = new Vector2Int(i,j);
                
                _slotsMap[position] = slot;
            }
        }
    }
    
    public IReadOnlyInventorySlot<Sprite, int>[,] GetSlots()
    {
        var array = new IReadOnlyInventorySlot<Sprite, int>[Size.x, Size.y];

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var position = new Vector2Int(i, j);
                array[i, j] = _slotsMap[position];
            }
        }

        return array;
    }

    public AddItemsToInventoryGridResult AddItem(string itemId, int amount = 1)
    {
        var remainingAmount = amount;
        var itemsAddedToSlotsWithSameItemsAmount =
            AddToSlotsWithSameItems(itemId, remainingAmount, out remainingAmount);
        if (remainingAmount <= 0)
        {
            return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedToSlotsWithSameItemsAmount);
        }

        var itemsAddedToAvailableSlotsAmount = AddToFirstAvailableSlots(itemId, remainingAmount, out remainingAmount);
        var totalAddedItemsAmount = itemsAddedToSlotsWithSameItemsAmount + itemsAddedToAvailableSlotsAmount;
        
        return new AddItemsToInventoryGridResult(OwnerId, amount, totalAddedItemsAmount);
    }

    public void AddToFirstAvailableSlot(string itemId,int price, int amount = 1)
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var coords = new Vector2Int(i, j);
                var slot = _slotsMap[coords];
                
                if (!slot.IsEmpty)
                {
                    continue;
                }
                
                slot.ItemId = itemId;
                slot.Amount += amount;
                slot.Price = price;
                return;
            }
        }
    }

    private int AddToFirstAvailableSlots(string itemId, int amount, out int remainingAmount)
    {
        var itemsAddedAmount = 0;
        remainingAmount = amount;
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var coords = new Vector2Int(i, j);
                var slot = _slotsMap[coords];
                
                if (!slot.IsEmpty)
                {
                    continue;
                }

                slot.ItemId = itemId;
                var newValue = remainingAmount;
                var slotItemCapacity = GetItemSlotCapacity(slot.ItemId);

                if (newValue > slotItemCapacity)
                {
                    remainingAmount = newValue - slotItemCapacity;
                    var itemsToAddAmount = slotItemCapacity;
                    itemsAddedAmount += itemsToAddAmount;
                    slot.Amount = slotItemCapacity;
                }
                else
                {
                    itemsAddedAmount += remainingAmount;
                    slot.Amount += newValue;

                    remainingAmount = 0;
                    return itemsAddedAmount;
                }
            }
        }

        return itemsAddedAmount;
    }

    private int AddToSlotsWithSameItems(string itemId, int amount, out int remainingAmount)
    {
        var itemsAddedAmount = 0;
        remainingAmount = amount;
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var coords = new Vector2Int(i, j);
                var slot = _slotsMap[coords];
                var slotItemCapacity = GetItemSlotCapacity(slot.ItemId);
                
                if (slot.IsEmpty || slot.Amount >= slotItemCapacity || slot.ItemId != itemId)
                {
                    continue;
                }

                var newValue = slot.Amount + remainingAmount;

                if (newValue > slotItemCapacity)
                {
                    remainingAmount = newValue - slotItemCapacity;
                    var itemsToAddAmount = slotItemCapacity - slot.Amount;
                    itemsAddedAmount += itemsToAddAmount;
                    slot.Amount = slotItemCapacity;

                    if (remainingAmount == 0)
                    {
                        return itemsAddedAmount;
                    }
                }
                else
                {
                    itemsAddedAmount += remainingAmount;
                    slot.Amount += newValue;

                    remainingAmount = 0;
                    return itemsAddedAmount;
                }
            }
        }

        return itemsAddedAmount;
    }

    public AddItemsToInventoryGridResult AddItem(Vector2Int slotCoords, string itemId, int amount = 1)
    {
        var slot = _slotsMap[slotCoords];
        var newValue = slot.Amount + amount;
        var itemsAddedAmount = 0;
        
        if (slot.IsEmpty)
        {
            slot.ItemId = itemId;
        }

        var itemSlotCapacity = GetItemSlotCapacity(itemId);

        if (newValue > itemSlotCapacity)
        {
            var remainingItems = newValue - itemSlotCapacity;
            var itemsToAddAmount = itemSlotCapacity - slot.Amount;
            itemsAddedAmount += itemsToAddAmount;
            slot.Amount = itemSlotCapacity;

            var result = AddItem(itemId, remainingItems);
            itemsAddedAmount += result.ItemsAddedAmount;
        }
        else
        {
            itemsAddedAmount = amount;
            slot.Amount = newValue;
        }
        
        return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedAmount);
    }
    
    public RemoveItemsFromInventoryGridResult RemoveItems(string itemId, int amount = 1)
    {
        if (!Has(itemId, amount))
        {
            return new RemoveItemsFromInventoryGridResult(OwnerId, amount, false);
        }

        var amountToRemove = amount;

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var coords = new Vector2Int(i, j);
                var slot = _slotsMap[coords];

                if (slot.ItemId != itemId)
                {
                    continue;
                }

                if (amountToRemove > slot.Amount)
                {
                    amountToRemove -= slot.Amount;

                    RemoveItems(coords, itemId, slot.Amount);

                }
                else
                {
                    RemoveItems(coords, itemId, amountToRemove);
                    return new RemoveItemsFromInventoryGridResult(OwnerId, amount, true);
                }
            }
        }
        
        throw new Exception("Something went wrong, couldn't remove some items");
    }

    public RemoveItemsFromInventoryGridResult RemoveItems(Vector2Int slotCoords, string itemId, int amount = 1)
    {
        var slot = _slotsMap[slotCoords];

        if (slot.IsEmpty || slot.ItemId != itemId || slot.Amount < amount)
        {
            return new RemoveItemsFromInventoryGridResult(OwnerId, amount, false);
        }

        slot.Amount -= amount;

        if (slot.Amount == 0)
        {
            slot.ItemId = null;
        }

        return new RemoveItemsFromInventoryGridResult(OwnerId, amount, true);
    }
    
    public int GetAmount(string itemId)
    {
        var amount = 0;
        var slots = _data.SlotsData;

        foreach (var slot in slots)
        {
            if (slot.itemID == itemId)
            {
                amount += slot.amount;
            }
        }

        return amount;
    }

    public bool Has(string itemId, int amount)
    {
        var amountExist = GetAmount(itemId);
        return amountExist >= amount;
    }

    public void CleanInventory()
    {
        var slots = _data.SlotsData;
        foreach (var slot in slots)
        {
            RemoveItems(slot.itemID, slot.amount);
        }
    }

    public void SwitchSlots(Vector2Int slotCoordsA, Vector2Int slotCoordsB)
    {
        var slotA = _slotsMap[slotCoordsA];
        var slotB = _slotsMap[slotCoordsB];
        var tempSlotItemId = slotA.ItemId;
        var tempSlotAmount = slotA.Amount;

        slotA.ItemId = slotB.ItemId;
        slotA.Amount = slotB.Amount;
        slotB.ItemId = tempSlotItemId;
        slotB.Amount = tempSlotAmount;

    }
    
    private int GetItemSlotCapacity(string itemId)
    {
        return 1;
    }
}