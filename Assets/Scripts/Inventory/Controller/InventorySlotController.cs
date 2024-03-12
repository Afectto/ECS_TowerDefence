using UnityEngine;

public class InventorySlotController
{
    private readonly InventorySlotView _view;

    public InventorySlotController(IReadOnlyInventorySlot<Sprite, int> slot, InventorySlotView view)
    {
        _view = view;

        slot.ItemChange += onSlotItemIconChange;
        slot.ItemNumberChange += onSlotItemPriceChanged;

        view.Icon = slot.Item;
        view.Price = slot.Amount;
    }

    private void onSlotItemIconChange(Sprite newIconSprite)
    {
        _view.Icon = newIconSprite;
    }

    private void onSlotItemPriceChanged(int newPrice)
    {
        _view.Price = newPrice;
    }
}
