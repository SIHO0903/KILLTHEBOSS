using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueController : Interact
{
    [SerializeField] Transform[] images;
    [SerializeField] Button[] buttons;


    int[] indexs;
    public const float HEALTH_INCREASE = 25;
    public const float HEALTHRECOVERY_INCREASE = 0.1f;
    public const float DAMAGE_INCREASE = 1f;
    public const float DEFENSE_INCRESE = 1f;
    public const float POTIONCOOLTIONE_INCREASE = -2f;
    private void Awake()
    {
        StatueData loadData = JsonSaveLoader.Statue_Load();
        indexs = new int[buttons.Length];
        if (loadData != null)
        {
            for (int i = 0; i < loadData.ints.Count; i++)
            {
                indexs[i] = loadData.ints[i];
                LoadUpgradeImg(images[i], buttons[i], indexs[i]);
            }

        }

        buttons[0].onClick.AddListener(() =>
        {

            UpdateUpgradeImg(images[0], buttons[0], indexs[0]++, inventory.GetComponent<PlayerController>().HealthUpgrade,HEALTH_INCREASE);
            //UpgradeStatus(inventory.GetComponent<PlayerController>().HealthUpgrade, HEALTH_INCREASE);
        });

        buttons[1].onClick.AddListener(() =>
        {
            UpdateUpgradeImg(images[1], buttons[1], indexs[1]++,inventory.GetComponent<PlayerController>().ADDHealthRecoverySpeed,HEALTHRECOVERY_INCREASE);
            //UpgradeStatus(inventory.GetComponent<PlayerController>().ADDHealthRecoverySpeed, HEALTHRECOVERY_INCREASE);
        });
        buttons[2].onClick.AddListener(() =>
        {
            UpdateUpgradeImg(images[2], buttons[2], indexs[2]++, inventory.GetComponent<WeaponController>().ADD_Damage, DAMAGE_INCREASE);
            //UpgradeStatus(inventory.GetComponent<WeaponController>().ADD_Damage, DAMAGE_INCREASE);
        });
        buttons[3].onClick.AddListener(() =>
        {
            UpdateUpgradeImg(images[3], buttons[3], indexs[3]++, inventory.GetComponent<PlayerController>().UpgradeDefenfse, DEFENSE_INCRESE);
            //UpgradeStatus(inventory.GetComponent<PlayerController>().UpgradeDefenfse, DEFENSE_INCRESE);
        });
        buttons[4].onClick.AddListener(() =>
        {
            UpdateUpgradeImg(images[4], buttons[4], indexs[4]++, inventory.AddPotionCoolTime, POTIONCOOLTIONE_INCREASE);
            //UpgradeStatus(inventory.AddPotionCoolTime, POTIONCOOLTIONE_INCREASE);
        });


    }


    public static void Statue_StatsLoad()
    {
        StatueData loadData = JsonSaveLoader.Statue_Load();
        if (loadData == null) return;
        InventoryController inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        PlayerController playerController = inventory.GetComponent<PlayerController>();
        WeaponController weaponController = inventory.GetComponent<WeaponController>();
        UpgradeStatus(playerController.HealthUpgrade, HEALTH_INCREASE * loadData.ints[0]);
        UpgradeStatus(playerController.ADDHealthRecoverySpeed, HEALTHRECOVERY_INCREASE * loadData.ints[1]);
        UpgradeStatus(weaponController.ADD_Damage, DAMAGE_INCREASE * loadData.ints[2]);
        UpgradeStatus(playerController.UpgradeDefenfse, DEFENSE_INCRESE * loadData.ints[3]);
        UpgradeStatus(inventory.AddPotionCoolTime, POTIONCOOLTIONE_INCREASE * loadData.ints[4]);
    }

    static void UpgradeStatus(Action<float> action, float status)
    {
        action?.Invoke(status);
    }

    void UpdateUpgradeImg(Transform images, Button button, int index, Action<float> action, float status)
    {
        Color color = new Color(125, 0, 0, 255);


        for (int i = 0; i < images.childCount; i++)
        {
            Image image = images.GetChild(i).GetComponent<Image>();
            if (image.color == Color.white)
            {
                if (GameManager.instance.Money < 5000 * (i + 1))
                    return;
                image.color = color;
                UpgradeStatus(action, status);
                GameManager.instance.Money -= 5000 * (i + 1);
                SoundManager.instance.PlaySound(SoundType.Upgrade);
                if (index >= 4)
                {
                    Debug.Log("최대레벨입니다");
                    button.interactable = false;
                }
                break;

            }
        }

        JsonSaveLoader.Statue_Save(indexs);
    }
    void LoadUpgradeImg(Transform images,Button button, int index)
    {
        Color color = new Color(125, 0, 0, 255);

        for (int i = 0; i < index; i++)
        {
            Image image = images.GetChild(i).GetComponent<Image>();
            if (image.color == Color.white)
            {
                image.color = color;
            }
            if (index > 4)
            {
                Debug.Log("최대레벨입니다");
                button.interactable = false;
            }
        }
    }
}
