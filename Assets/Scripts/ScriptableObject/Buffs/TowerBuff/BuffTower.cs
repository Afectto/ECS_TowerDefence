using System;
using UnityEngine;

[Serializable]
public struct BuffTower
{
    public Sprite Icon;
    public int Price;
    public int Value;

    public BuffTower(Sprite icon, int price, int value)
    {
        Icon = icon;
        Price = price;
        Value = value;
    }
}