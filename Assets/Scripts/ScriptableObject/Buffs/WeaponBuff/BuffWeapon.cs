using System;
using UnityEngine;

[Serializable]
public struct BuffWeapon
{
    public Sprite Icon;
    public int Price;
    public float Value;
    public TypeWeaponBuff TypeWeaponBuff;
    
    public BuffWeapon(Sprite icon, int price, float value, TypeWeaponBuff typeWeaponBuff)
    {
        Icon = icon;
        Price = price;
        Value = value;
        TypeWeaponBuff = typeWeaponBuff;
    }
}