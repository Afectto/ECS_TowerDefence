public class ScreenController
{
    private readonly InventoryService _inventoryService;
    private readonly ScreenView _view;

    private InventoryGridController _inventoryController;

    public ScreenController(InventoryService inventoryService, ScreenView view)
    {
        _inventoryService = inventoryService;
        _view = view;
    }

    public void OpenInventory(string ownerId)
    {
        var inventory = _inventoryService.GetInventory(ownerId);
        var inventoryView = _view.InventoryView;
        
        _inventoryController = new InventoryGridController(inventory, inventoryView);

    }
}
