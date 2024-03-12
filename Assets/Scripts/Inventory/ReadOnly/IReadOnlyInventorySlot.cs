using System;

public interface IReadOnlyInventorySlot<TItem, TNumber>
{
    event Action<TItem> ItemChange;
    event Action<TNumber> ItemNumberChange;
    
    TItem Item { get; }
    TNumber Amount { get; }
    bool IsEmpty { get; }
}
