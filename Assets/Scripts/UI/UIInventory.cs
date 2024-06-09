using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIInventory : MonoBehaviour
{
    [HideInInspector]
    public List<UIItem> equips = new List<UIItem>();
    [HideInInspector]
    public List<UIItem> items = new List<UIItem>();
    List<UIItem> usings = new List<UIItem>();
    List<UIItem> tmpList = new List<UIItem>();

    public event Action<UIItem> UseItemAction;
    public event Action CurrentEquipEffectsCheck;
    [SerializeField] RectTransform equipBox;
    [SerializeField] RectTransform itemBox;
    [SerializeField] RectTransform usingBox;

    [SerializeField] UIDescription description;
    [SerializeField] UIDragPanel dragPanel;

    int currentDragIndex = -1;
    UIItem currentAmmoItem;
    private void Awake()
    {
        Toggle(true);

        Init(equips, equipBox);
        Init(items, itemBox);
        Init(usings, usingBox);

        for (int i = 0; i < usings.Count; i++)
        {
            usings[i].InitUsingTxt(i + 1);
        }


        Toggle(false);
    }
    private void Start()
    {
        LoadData();
        CurrentEquipEffectsCheck?.Invoke();
    }
    public void SaveData()
    {
        JsonSaveLoader.Inventory_Save(equips, items, usings);

    }
    void LoadData()
    {
        InventoryData loadData = JsonSaveLoader.Inventory_Load();
        StatueController.Statue_StatsLoad();
        if(loadData == null)
        {

            return;
        }
        else
        {
            foreach (var item in loadData.values)
            {
                if (item.index == 0)
                {
                    equips[item.key].SetData(ItemManager.instance.GetItemByItemID(item.itemID), item.quantity);
                }
                else if (item.index == 1)
                {
                    items[item.key].SetData(ItemManager.instance.GetItemByItemID(item.itemID), item.quantity);
                }
                else if (item.index == 2)
                {
                    usings[item.key].SetData(ItemManager.instance.GetItemByItemID(item.itemID), item.quantity);
                }
            }
        }

    }
    void Init(List<UIItem> items, RectTransform box)
    {
        items.Capacity = box.childCount;
        for (int i = 0; i < items.Capacity; i++)
        {
            items.Add(box.GetChild(i).GetComponent<UIItem>());
            items[i].ResetData();
            items[i].PointerRightClick += OnRightClick;
            items[i].PointerShiftRightClick += OnShiftRigthClick;
            items[i].PointerMove += DescriptionShow;
            items[i].PointerExit += DescriptionHide;
            items[i].BeginDrag += BeginDrag;
            items[i].EndDrag += EndDrag;
            items[i].Drop += Drop;
        }
    }

    void OnShiftRigthClick(UIItem uiItem)
    {
        if (Shop.IsShopOn)
        {
            Debug.Log("판매");
            uiItem.SellItem();
            SoundManager.instance.PlaySound(SoundType.SellItem);
        }

    }
    void OnRightClick(UIItem uiItem)
    {
        int index = items.IndexOf(uiItem);
        switch (uiItem.item.Type)
        {
            case ItemSO.TypeEnum.Helmet:
                ItemSwap(items, equips, index, 0);
                break;
            case ItemSO.TypeEnum.Armor:
                ItemSwap(items, equips, index, 1);
                break;
            case ItemSO.TypeEnum.Boots:
                ItemSwap(items, equips, index, 2);
                break;
            case ItemSO.TypeEnum.Accessories:
                ItemSwap(items, equips, index, 3);
                break;
            case ItemSO.TypeEnum.Consumable:
                UseItemAction?.Invoke(uiItem);
                uiItem.Updatequantity(-1);
                break;
        }

    }



    UIDescription DescriptionShow(UIItem uiItem)
    {
        ItemSO item = uiItem.item;
        description.SetData(description.Desc(item));
        return description;
    }
    void DescriptionHide(UIItem uiItem)
    {
        description.Toggle(false);
    }

    private void BeginDrag(UIItem uiItem)
    {
        int index = GetList(uiItem).IndexOf(uiItem);
        currentDragIndex = index;
        tmpList = GetList(uiItem);
        dragPanel.SetData(uiItem.item.itemImage, uiItem.Quantity);
        dragPanel.Toggle(true);
    }
    private void Drop(UIItem uiItem)
    {
        ItemSwap(tmpList, GetList(uiItem), currentDragIndex, GetList(uiItem).IndexOf(uiItem));
        currentDragIndex = -1;
    }


    public List<UIItem> GetList(UIItem uiItem)
    {
        if (equips.Contains(uiItem))
            return equips;
        else if (items.Contains(uiItem))
            return items;
        else if (usings.Contains(uiItem))
            return usings;
        else
            return null;
    }

    public UIItem InventoryIngredientCheck(ItemSO selectedItem)
    {

        int ingredientID = selectedItem.ingredient[0].ingredient.ID;

        foreach (var item in items)
        {
            if (item.item == null)
                continue;
            else if (item.item.ID == ingredientID)
                return item;

        }
        foreach (var item in usings)
        {
            if (item.item == null)
                continue;
            else if (item.item.ID == ingredientID)
                return item;
        }
        Debug.Log("nullllllll");
        return null;
    }

    private void EndDrag(UIItem uiItem)
    {
        dragPanel.Toggle(false);
    }

    UIItem HasPotionCheck()
    {

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != null && items[i].item.Type == ItemSO.TypeEnum.Consumable)
            {
                Debug.Log("포션찾음 저장 : " + items[i].item.name);
                return items[i];
            }
        }


        return null;
    }

    public void PotionConsume()
    {
        UseItemAction?.Invoke(HasPotionCheck());
        
    }

    public UIItem AmmoItemCheck()
    {

        if (currentAmmoItem == null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].item != null)
                {
                    if (items[i].item.Type == ItemSO.TypeEnum.Ammo)
                    {
                        currentAmmoItem = items[i];
                    }
                }

                Debug.Log("포문도는거 체크");
            }
        }

        return currentAmmoItem;
    }

    public void UseAmmo(UIItem ammoItem)
    {
        ammoItem.Updatequantity(-1);
    }
    public void ItemSwap(List<UIItem> item1, List<UIItem> item2, int index1, int index2)
    {
        while (item1[index1].item.Type == ItemSO.TypeEnum.Accessories && !item2[index2].empty)
        {
            index2 += 1;
            if (index2 >= item2.Count)
            {
                index2 = 3;
                break;
            }
        }

        if (!item2[index2].empty)
        {

            ItemSO tmpItem = item2[index2].item;
            int tmpQuantity = item2[index2].Quantity;

            item2[index2].SetData(item1[index1].item.itemImage, item1[index1].Quantity, item1[index1].item);
            item1[index1].SetData(tmpItem.itemImage, tmpQuantity, tmpItem);

        }
        else
        {
            item2[index2].SetData(item1[index1].item.itemImage, item1[index1].Quantity, item1[index1].item);
            item1[index1].ResetData();
        }

        for (int i = 0; i < equips.Count; i++)
        {
            equips[i].ToggleQuantityTxt(false);
        }
        CurrentEquipEffectsCheck?.Invoke();
    }

    public void Toggle(bool val)
    {
        gameObject.GetComponent<VerticalLayoutGroup>().padding.top = val == true ? -150 : 0;
        equipBox.parent.gameObject.SetActive(val);
        itemBox.parent.gameObject.SetActive(val);
        currentAmmoItem = null;
    }

    public void GetItem(ItemSO item,int quantity=1)
    {
        SoundManager.instance.PlaySound(SoundType.GetItem);
        if (item.name == "Money")
        {
            GameManager.instance.Money += quantity;
            return;
        }


        //이미잇는거에서 수량추가
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != null)
            {
                if (items[i].item.ID == item.ID && item.IsStackable)
                {

                    items[i].Updatequantity(quantity);
                    return;
                }
            }

        }

        for (int i = 0; i < usings.Count; i++)
        {
            if (usings[i].item != null)
            {
                if (usings[i].item.ID == item.ID && item.IsStackable)
                {
                    usings[i].Updatequantity(quantity);
                    return;
                }
            }
        }
        for (int i = 0; i < items.Count; i++)
        {

            if (items[i].empty)
            {
                items[i].SetData(item.itemImage, quantity, item);
                return;
            }


        }

        //for (int i = 0; i < items.Count; i++)
        //{
        //    //if (items[i].item != null)
        //    if (items[i].item.ID == item.ID)
        //    {
        //        items[i].Updatequantity(1);
        //        return;
        //    }

        //    if (!items[i].empty)
        //    {
        //        continue;
        //    }

        //    items[i].SetData(item.itemImage, 1, item);
        //    return;
        //}
    }

    public ItemSO ItemSelected(int index)
    {
        for (int i = 0; i < usings.Count; i++)
        {
            usings[i].ItemSelected(false);
        }
        //index = index < 0 ? -index : index;
        //index = index % usings.Count;

        index = (int)Mathf.Repeat(index, 6);
        usings[index].ItemSelected(true);

        return usings[index].item;
    }
}
