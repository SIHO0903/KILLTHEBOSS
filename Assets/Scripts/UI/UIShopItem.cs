using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemCost;
    CanvasGroup canvasGroup;

    public event Action<UIShopItem> ItemClicked;

    public ItemSO itemSO;
    private void Awake()
    {
       canvasGroup = GetComponent<CanvasGroup>();
        SetData(itemSO);
    }


    public void SetData(ItemSO itemSO)
    {
        this.itemSO = itemSO;
        itemImage.sprite = itemSO.itemImage;
        itemName.text = itemSO.Name;
        itemCost.text = GetThousandCommaText(itemSO.BuyCost);
        canvasGroup.blocksRaycasts = true;

    }
    public string GetThousandCommaText(int data) 
    { 
        return string.Format("{0:#,###,###}", data); 
    }

    public ItemSO BuyItem()
    {
        //itemCost.text = "Sold!";
        //canvasGroup.blocksRaycasts = false;
        
        return itemSO;
    }


    public void OnPointerClick(PointerEventData eventData)
    {    
        ItemClicked?.Invoke(this);
    }
}
