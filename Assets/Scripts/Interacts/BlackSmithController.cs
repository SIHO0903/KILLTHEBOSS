using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackSmithController : Shop
{
    [Header("Main")]
    [SerializeField] RectTransform shop;
    [SerializeField] RectTransform weaponCraft;
    [SerializeField] Button shopBtn;
    [SerializeField] Button craftBtn;

    [Space(20)]
    [Header("Craft")]
    [SerializeField]Transform craftTable;
    [SerializeField] Image craftImage;
    [SerializeField] Image craftIngredient1_Image;
    [SerializeField] Image craftIngredient2_Image;
    [SerializeField] TMP_Text craftIngredient1_Txt;
    [SerializeField] TMP_Text craftIngredient2_Txt;
    [SerializeField] Button itemCraftBtn;

    ItemSO getItem;

    ScrollRect scrollRect;

    List<UIShopItem> crafts = new List<UIShopItem>();
    public override void Awake()
    {
        base.Awake();
        scrollRect = shopUI.GetComponent<ScrollRect>();
        Init(crafts, weaponCraft,CraftTable);

        shopBtn.onClick.AddListener(() => 
        {
            shop.gameObject.SetActive(true);
            weaponCraft.gameObject.SetActive(false);
            craftTable.gameObject.SetActive(false);
            scrollRect.content = shop;
        });
        craftBtn.onClick.AddListener(() =>
        {
            weaponCraft.gameObject.SetActive(true);
            shop.gameObject.SetActive(false);
            craftTable.gameObject.SetActive(false);
            scrollRect.content = weaponCraft;
        });

        itemCraftBtn.onClick.AddListener(() =>
        {
            itemCraftBtn.interactable = false;
            Craft();
        });

    }
    public void CraftTable(UIShopItem shopItem)
    {

        int index = crafts.IndexOf(shopItem);
        craftTable.gameObject.SetActive(true);
        ShowInfo(crafts[index].itemSO); //클릭된 아이템의 정보를 가져옴

    }
    public void ShowInfo(ItemSO currentItem)
    {

        UIItem currentInventoryItem = InventoryIngredientCheck(currentItem); // 해당무기의 재료로쓰는 인벤토리의 아이템을 받아옴
        int currentQuantity;
        if (currentInventoryItem == null)
            currentQuantity = 0;
        else
            currentQuantity = currentInventoryItem.Quantity;

        getItem = currentItem;

        craftImage.sprite = currentItem.itemImage;
        craftIngredient1_Image.sprite = currentItem.ingredient[0].ingredient.itemImage;
        craftIngredient2_Image.sprite = currentItem.ingredient[1].ingredient.itemImage;

        craftIngredient1_Txt.color = IsIngredientEnough(currentQuantity, currentItem.ingredient[0].count);
        craftIngredient2_Txt.color = IsIngredientEnough(GameManager.instance.Money, currentItem.ingredient[1].count);

        craftIngredient1_Txt.text = currentQuantity + " / " + currentItem.ingredient[0].count;
        craftIngredient2_Txt.text = MyUtils.GetThousandCommaText(GameManager.instance.Money) + " / " + 
                                    MyUtils.GetThousandCommaText(currentItem.ingredient[1].count); // 소지중인금액 / 필요한금액

        if (craftIngredient1_Txt.color == Color.green && craftIngredient2_Txt.color == Color.green)
        {
            itemCraftBtn.interactable = true;
        }
        else
            itemCraftBtn.interactable = false;
    }
    public void Craft()
    {
        UIItem currentInventoryItem = InventoryIngredientCheck(getItem); // 해당무기의 재료로쓰는 인벤토리의 아이템을 받아옴

        currentInventoryItem.Quantity -= getItem.ingredient[0].count;
        GameManager.instance.Money -= getItem.ingredient[1].count;

        currentInventoryItem.Updatequantity(-getItem.ingredient[0].count);
        inventory.GetItem(getItem);
        ShowInfo(getItem);
    }
    Color IsIngredientEnough(int currentQuantity, int neededQuantity)
    {
        if (currentQuantity < neededQuantity)
            return Color.red;
        else
            return Color.green;
    }
    public UIItem InventoryIngredientCheck(ItemSO item)
    {
        return inventory?.CraftIngredientCheck(item);
    }

}
