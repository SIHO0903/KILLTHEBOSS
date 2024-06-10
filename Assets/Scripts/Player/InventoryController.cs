using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [SerializeField] UIInventory inventory;
    WeaponController weaponController;
    int itemIndex;
    bool toggle;
    float curPotionCoolTime;
    float basePotionCoolTime =45f;
    float PotionCoolTime { get; set; } //아직 안씀
    float autoSaveTimer;
    float autoSaveTimerCycle = 300f;
    [SerializeField] Image potionCoolTimeImage;
    private void Start()
    {
        weaponController = GetComponent<WeaponController>();
        inventory.UseItemAction += UesItem;
        inventory.CurrentEquipEffectsCheck += CurrentEquipEffects;
        PotionCoolTime += basePotionCoolTime;
        curPotionCoolTime = PotionCoolTime;
        CurrentEquipEffects();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            toggle = !toggle;
            inventory.Toggle(toggle);
        }

        ItemChange();

        Potion();

        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveData();
        }

        AutoSave();
    }

    private void Potion()
    {
        if (curPotionCoolTime <= PotionCoolTime)
        {
            curPotionCoolTime += Time.deltaTime;
            potionCoolTimeImage.fillAmount = 1 - curPotionCoolTime / PotionCoolTime;
        }
        else
        {
            potionCoolTimeImage.transform.parent.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.H) && curPotionCoolTime >= PotionCoolTime)
        {
            potionCoolTimeImage.transform.parent.gameObject.SetActive(true);
            inventory.PotionConsume();
            SoundManager.instance.PlaySound(SoundType.UsePotion);
            curPotionCoolTime = 0;
        }
    }

    void AutoSave()
    {
        autoSaveTimer += Time.deltaTime;
        if (autoSaveTimer >= autoSaveTimerCycle)
        {

            autoSaveTimer = 0;
        }
    }
    public void SaveData()
    {
        inventory.SaveData();
    }
    private void ItemChange()
    {
        if (!toggle)
        {
            if (Input.GetKeyDown(GetKeyPressed1to6()))
            {
                itemIndex = (int)GetKeyPressed1to6() - 49;
                weaponController.ChangeWeapon(inventory, itemIndex);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                weaponController.ChangeWeapon(inventory, ++itemIndex);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                weaponController.ChangeWeapon(inventory, --itemIndex);
            }
        }

    }

    public void CurrentEquipEffects()
    {
        Debug.Log("착용중인아이템체크후 업데이트");
        gameObject.GetComponent<PlayerController>().CurrentEquipEffects(inventory.equips);
    }


    public void GetItem(ItemSO item)
    {
        inventory.GetItem(item);
    }

    public UIItem CraftIngredientCheck(ItemSO item)
    {
        return inventory.InventoryIngredientCheck(item);
    }

    public void UesItem(UIItem UIItem)
    {
        UIItem.item.effects[0]?.Apply(gameObject.GetComponent<PlayerController>());
        UIItem.Updatequantity(-1);
    }

    public UIItem AmmoItemCheck()
    {
        return inventory.AmmoItemCheck();
    }
    public void UseAmmo(UIItem ammoItem)
    {
        inventory.UseAmmo(ammoItem);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            inventory.GetItem(item.item,item.Quantity);

            collision.gameObject.SetActive(false);
        }
    }


    private KeyCode GetKeyPressed1to6()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) return KeyCode.Alpha1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) return KeyCode.Alpha2;
        if (Input.GetKeyDown(KeyCode.Alpha3)) return KeyCode.Alpha3;
        if (Input.GetKeyDown(KeyCode.Alpha4)) return KeyCode.Alpha4;
        if (Input.GetKeyDown(KeyCode.Alpha5)) return KeyCode.Alpha5;
        if (Input.GetKeyDown(KeyCode.Alpha6)) return KeyCode.Alpha6;
        return KeyCode.None;
    }

    public void AddPotionCoolTime(float amount)
    {
        PotionCoolTime += amount;
    }
}


