using System;
using UnityEngine;

public class InventorySlot : IReadOnlyInventorySlot<Sprite, int>
{
    public event Action<Sprite> ItemChange;
    public event Action<int> ItemNumberChange;
    private Sprite _sprite;
    
    public string ItemId
    {
        get => _data.itemID;
        set
        {
            if (_data.itemID != value)
            {
                _data.itemID = value;
                var path = "Inventory/BaseBuff/" + value;
                Item = Resources.Load<Sprite>(path);
                ItemChange?.Invoke(Item);
            }
        }
    }
    public int Amount {        
        get => _data.amount;
        set
        {
            if (_data.amount != value)
            {
                _data.amount = value;
            }
        }
    }

    private int _currentPrice;
    public int Price
    {
        get => _currentPrice;
        set
        {
            _currentPrice = value;
            ItemNumberChange?.Invoke(value);
        }
    }
    
    public Sprite Item
    {
        get => _sprite;
        set
        {
            _sprite = value;
        }
    }
    
    public bool IsEmpty => Amount == 0 && string.IsNullOrEmpty(ItemId);

    private readonly InventorySlotData _data;

    public InventorySlot(InventorySlotData data)
    {
        _data = data;
    }

}