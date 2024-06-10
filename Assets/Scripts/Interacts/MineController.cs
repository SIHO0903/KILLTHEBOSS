using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OfflineData
{
    public float collectedMoney;
    public float currentIncreaseSpeed;
    public float nextIncreaseSpeed;
    public float nextUpgradeCost;
    public string oldTime;
}
public class MineController : Interact
{
    [SerializeField] Button upgradeBtn;
    [SerializeField] TMP_Text upgradeCostTxt;
    [SerializeField] TMP_Text increaseSpeedTxt;
    [SerializeField] TMP_Text currentCollectedMoneyTxt;
    [SerializeField] Button collectBtn;

    [Header("Upgrade")]
    public float increase;
    public float inceaseUpgradeCost;


    float collectedMoney;
    float currentIncreaseSpeed = 1f;
    float nextIncreaseSpeed = 1f;
    float nextUpgradeCost;
    string oldTime = null;


    private void Awake()
    {
        upgradeBtn.onClick.AddListener(() =>
        {
            currentIncreaseSpeed = nextIncreaseSpeed;
            nextIncreaseSpeed += 0.1f;
            GameManager.instance.Money -= (int)nextUpgradeCost;
            nextUpgradeCost += 10;
            MineSave();

        });
        collectBtn.onClick.AddListener(() =>
        {
            GameManager.instance.Money += (int)collectedMoney;
            collectedMoney = 0;
            MineSave();
        });
    }
    private void OnEnable()
    {
        OfflineData data = JsonSaveLoader.Mine_Load();
        if(data != null)
        {
            Debug.Log("데이터가잇음");
            collectedMoney = data.collectedMoney;
            currentIncreaseSpeed = data.currentIncreaseSpeed;
            nextIncreaseSpeed = data.nextIncreaseSpeed;
            nextUpgradeCost = data.nextUpgradeCost;
            oldTime = data.oldTime;
        }
        if (oldTime == null)
            oldTime = DateTime.Now.ToBinary().ToString();

        var tempOfflineTime = Convert.ToInt64(oldTime);
        var oldTime_New = DateTime.FromBinary(tempOfflineTime);
        var currentTime_New = DateTime.Now;
        var difference = currentTime_New.Subtract(oldTime_New);
        var rawTime = (float)difference.TotalSeconds;

        collectedMoney += rawTime * currentIncreaseSpeed;
        //TimeSpan timer = TimeSpan.FromSeconds(rawTime);
        //Debug.Log(timer + "시간동안 자리를 비웟습니다.");

    }
    private void Update()
    {

        collectedMoney += Time.deltaTime * currentIncreaseSpeed;

        upgradeCostTxt.text = nextUpgradeCost+"";
        currentCollectedMoneyTxt.text = string.Format($"{collectedMoney:N0}");
        increaseSpeedTxt.text = string.Format($"Collecting speed\n {currentIncreaseSpeed:N1} -> {nextIncreaseSpeed:N1}");


        if (GameManager.instance.Money >= nextUpgradeCost)
            upgradeBtn.interactable = true;
        else
            upgradeBtn.interactable = false;

        if (collectedMoney >= 1f)
            collectBtn.interactable = true;
        else
            collectBtn.interactable = true;
    }
    void MineSave()
    {
        JsonSaveLoader.Mine_Save(collectedMoney, currentIncreaseSpeed, nextIncreaseSpeed, nextUpgradeCost, DateTime.Now.ToBinary().ToString());
    }
}
