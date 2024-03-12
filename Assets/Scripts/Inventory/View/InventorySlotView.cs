using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image _spriteIcon;
    [SerializeField] private Text _textPrice;
    
    public Sprite Icon
    {
        get => _spriteIcon.sprite;
        set => _spriteIcon.sprite = value;
    }
    
    public int Price
    {
        get => Convert.ToInt32(_textPrice.text);
        set => _textPrice.text = value.ToString();
    }
}