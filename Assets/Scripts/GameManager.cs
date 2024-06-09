using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MyUtils 
{
    public static string GetThousandCommaText(int data)
    {
        return string.Format("{0:#,###,###}", data);
    }
    public static bool WhatFloor(Vector3 startPos,float distance,Vector2 dir,Vector2 size, params string[] layerName)
    {
        int layerMask = 0;
        for (int i = 0; i < layerName.Length; i++)
            layerMask += 1 << LayerMask.NameToLayer(layerName[i]);

        if (Physics2D.BoxCast(startPos, size, 0, dir, distance, layerMask))
            return true;
        else
            return false;
    }
    public static bool WhatFloor(Vector3 pos, params string[] layerName)
    {
        float distance = 1f;
        Vector2 dir = Vector2.down;
        Vector2 size = Vector2.one / 2f;

        return WhatFloor(pos, distance, dir, size, layerName);
    }

    public static GameObject Instansiate(GameObject gameObject, Vector3 startPos, Quaternion quaternion, Transform transform)
    {
        GameObject select = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).gameObject.activeSelf)
            {
                select = transform.GetChild(i).gameObject;
                transform.GetChild(i).gameObject.transform.position = startPos;
                transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }

        if (select == null)
        {
            select = UnityEngine.Object.Instantiate(gameObject, startPos, quaternion, transform);
        }

        return select;
    }
}


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] TMP_Text moneyTxt;
    CanvasGroup canvasGroup;
    int money;
    bool isMoneyTxtCoroutineActive;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            JsonSaveLoader.Money_Save(money);
            canvasGroup.alpha = 1f;
            if (!isMoneyTxtCoroutineActive)
                StartCoroutine(UpdateMoneyTxt());
        }
    }

    IEnumerator UpdateMoneyTxt()
    {
        isMoneyTxtCoroutineActive = true;
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime;
            moneyTxt.text = MyUtils.GetThousandCommaText(Money);

            yield return new WaitForEndOfFrame();
        }
        //JsonSaveLoader.Money_Save(money);
        isMoneyTxtCoroutineActive = false;
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        canvasGroup = moneyTxt.GetComponentInParent<CanvasGroup>();
        money = JsonSaveLoader.Money_Load().money;


    }

}
