using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatueController : Interact
{
    [SerializeField] Transform[] images;
    [SerializeField] Button[] buttons;


    int[] currentLevel;
    public const float HEALTH_INCREASE = 25;
    public const float HEALTHRECOVERY_INCREASE = 0.1f;
    public const float DAMAGE_INCREASE = 1f;
    public const float DEFENSE_INCRESE = 1f;
    public const float POTIONCOOLTIONE_INCREASE = -2f;
    private void Awake()
    {
        StatueData loadData = JsonSaveLoader.Statue_Load();
        currentLevel = new int[buttons.Length];
        if (loadData != null)
        {
            for (int i = 0; i < loadData.ints.Count; i++)
            {
                currentLevel[i] = loadData.ints[i];
                LoadUpgradeImg(images[i], buttons[i], currentLevel[i]);
            }

        }
        Action[] UpgradeActions1 = new Action[]
        {
            () => inventory.GetComponent<PlayerController>().HealthUpgrade(HEALTH_INCREASE),
            () => inventory.GetComponent<PlayerController>().ADDHealthRecoverySpeed(HEALTHRECOVERY_INCREASE),
            () => inventory.GetComponent<PlayerController>().UpgradeDefenfse(DEFENSE_INCRESE),
            () => inventory.GetComponent<WeaponController>().ADD_Damage(DAMAGE_INCREASE),
            () => inventory.AddPotionCoolTime(POTIONCOOLTIONE_INCREASE)
        };
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => UpdateUpgradeImg(images[index], buttons[index], currentLevel[index]++, UpgradeActions1[index]));

        }
    }
    void UpdateUpgradeImg(Transform images, Button button, int index, Action action)
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
                UpgradeStatus(action);
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

        JsonSaveLoader.Statue_Save(currentLevel);
    }
    void LoadUpgradeImg(Transform images, Button button, int index)
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
    static void UpgradeStatus(Action<float> action, float status)
    {
        action?.Invoke(status);
    }
    void UpgradeStatus(Action action)
    {
        action?.Invoke();
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
}
