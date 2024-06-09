using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shop : Interact
{
    protected List<UIShopItem> items = new List<UIShopItem>();
    [SerializeField] Transform slotTransform;
    public static bool IsShopOn { get; private set; } = false;
    protected void Awake()
    {
        Init(items, slotTransform,ItemBuy);
        CheckShop += CheckShopOn;
    }
    public void CheckShopOn()
    {
        IsShopOn = shopUI.activeSelf;
    }
    public void Init(List<UIShopItem> items, Transform slot, Action<UIShopItem> action)
    {
        for (int i = 0; i < slot.childCount; i++)
        {
            items.Add(slot.GetChild(i).GetComponent<UIShopItem>());
            items[i].ItemClicked += action;
        }
    }

    public void ItemBuy(UIShopItem shopItem)
    {
        if (GameManager.instance.Money >= shopItem.itemSO.BuyCost)
        {
            GameManager.instance.Money -= shopItem.itemSO.BuyCost;
            inventory.GetItem(shopItem.BuyItem());
            SoundManager.instance.PlaySound(SoundType.BuyItem);
        }
        else
        {
            SoundManager.instance.PlaySound(SoundType.NotEnoughMoney);
            Debug.Log("돈이부족합니다. :" + GameManager.instance.Money);
        }

    }
}
