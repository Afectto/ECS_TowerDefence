using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ScreenView Inventory;
    [SerializeField] private Image ShopIcon;
    [SerializeField] private List<TowerBuff> TowerBuffs;
    [SerializeField] private List<TowerWeaponBuff> TowerWeaponBuffs;

    private InventoryService _inventoryService;
    private ScreenController _screenController;
    
    private const string OWNER = "Player";
    private bool _isInventoryOn;
    
    private void Start()
    {
        _isInventoryOn = false;
        // Inventory.gameObject.SetActive(false);

        _inventoryService = new InventoryService();

        var inventoryData = CreateInventoryGridData(OWNER, 2, 3);
        _inventoryService.RegisterInventory(inventoryData);
        
        _screenController = new ScreenController(_inventoryService, Inventory);
        _screenController.OpenInventory(OWNER);

        RefreshInventory();
        
    }

    private InventoryGridData CreateInventoryGridData(string ownerId, int sizeX, int sizeY)
    {
        var size = new Vector2Int(sizeX, sizeY);
        var createdInventorySlots = new List<InventorySlotData>();
        var length = sizeX + sizeY + 1;

        for (int i = 0; i < length; i++)
        {
            createdInventorySlots.Add(new InventorySlotData());
        }
        var createInventoryData = new InventoryGridData
        {
            OwnerID = ownerId,
            Size = size,
            SlotsData = createdInventorySlots
        };
        return createInventoryData;
    }

    private void RefreshInventory()
    {
        _inventoryService.CleanInventory(OWNER);
        CreateBuffTower();
        CreateWeaponTower();
    }
    
    private void CreateBuffTower()
    {
        for (int i = 0; i < 3; i++)
        {
            var rListBuff = Random.Range(0, 100);
            int rTowerBuffIndex;
            switch (rListBuff)
            {
                case <50:
                    rTowerBuffIndex = Random.Range(0, TowerBuffs.Count);
                    var rTowerBuffStat = TowerBuffs[rTowerBuffIndex];
                    var rBuffStat = rTowerBuffStat.GetRandomBuff();
                    _inventoryService.AddToFirstAvailableSlot(OWNER, rBuffStat.Icon.name ,rBuffStat.Price);
                    break;
                case >=50:
                    rTowerBuffIndex = Random.Range(0, TowerWeaponBuffs.Count);
                    var rTowerBuffWeapon = TowerWeaponBuffs[rTowerBuffIndex];
                    var rBuffWeapon = rTowerBuffWeapon.GetRandomBuff();
                    _inventoryService.AddToFirstAvailableSlot(OWNER, rBuffWeapon.Icon.name, rBuffWeapon.Price);
                    break;
            }
        }
    }

    private void CreateWeaponTower()
    {
    }

    public void Bug()
    {
        _isInventoryOn = !_isInventoryOn;
        Inventory.gameObject.SetActive(_isInventoryOn);
        ChangeBugImage(_isInventoryOn);
    }

    private void ChangeBugImage(bool isOpen)
    {
        string imageName = "Inventory/" + (isOpen ? "OpenChest" : "CloseChest");
        Sprite newSprite = Resources.Load<Sprite>(imageName);
        ShopIcon.sprite = newSprite;
    }
}
