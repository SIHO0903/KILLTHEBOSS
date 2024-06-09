using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public struct InventoryMiniData
{
    public int index;
    public int key;
    public int itemID;
    public int quantity;

}

[System.Serializable]
public class InventoryData
{
    [SerializeField] public List<InventoryMiniData> values = new List<InventoryMiniData>();

    public void AddData(int i, int j, int itemID, int quantity)
    {
        InventoryMiniData miniData;
        miniData.index = i;
        miniData.key = j;
        miniData.itemID = itemID;
        miniData.quantity = quantity;
        values.Add(miniData);
    }

}
[System.Serializable]
public class StatueData
{
    public List<int> ints = new List<int>();
}
[System.Serializable]
public struct MoneyData
{
    public int money;
}

[System.Serializable]
public struct VolumData
{
    public float bgm;
    public float sfx;

    public static VolumData Default => new VolumData { bgm = 0.5f, sfx = 0.5f };
}
public class JsonSaveLoader : MonoBehaviour
{
    public static void Mine_Save(float collectedMoney,float currentIncreaseSpeed,float nextIncreaseSpeed, float nextUpgradeCost,string dateTime)
    {
        OfflineData data = new OfflineData();
        data.collectedMoney = collectedMoney;
        data.currentIncreaseSpeed = currentIncreaseSpeed;
        data.nextIncreaseSpeed = nextIncreaseSpeed;
        data.nextUpgradeCost = nextUpgradeCost;
        data.oldTime = dateTime;

        //Debug.Log("SaveData()");
        //Debug.Log("collectedMoney : " + collectedMoney);
        //Debug.Log("currentIncreaseSpeed : " + currentIncreaseSpeed);
        //Debug.Log("nextIncreaseSpeed : " + nextIncreaseSpeed);
        //Debug.Log("nextUpgradeCost : " + nextUpgradeCost);

        string json = JsonUtility.ToJson(data);
        //Debug.Log("Mine_Save : " + json);
        File.WriteAllText(Application.persistentDataPath + "/MineSave.json", json);
        //Debug.Log("Application.persistentDataPath : " + Application.persistentDataPath);
    }
    public static OfflineData Mine_Load()
    {
        string path = Application.persistentDataPath + "/MineSave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            OfflineData data = JsonUtility.FromJson<OfflineData>(json);

            return data;
            //Debug.Log("LoadData()");
            //Debug.Log("collectedMoney : " + collectedMoney);
            //Debug.Log("currentIncreaseSpeed : " + currentIncreaseSpeed);
            //Debug.Log("nextIncreaseSpeed : " + nextIncreaseSpeed);
            //Debug.Log("nextUpgradeCost : " + nextUpgradeCost);
        }
        return null;
    }
    public static void Inventory_Save(params List<UIItem>[] _Lsit_items)
    {
        InventoryData data = new InventoryData();

        for (int i = 0; i < _Lsit_items.Length; i++)
        {
            for (int j = 0; j < _Lsit_items[i].Count; j++)
            {
                if (_Lsit_items[i][j].item == null)
                    continue;

                data.AddData(i,j, _Lsit_items[i][j].item.ID, _Lsit_items[i][j].Quantity);

                Debug.Log("\n" + i + "번째 아이템리스트의 " + j + " 번째에 : " + _Lsit_items[i][j].item.Name + " 이 저장되었습니다.");
            }
        }
        string json = JsonUtility.ToJson(data);
        Debug.Log("Inventory_Save : " + json);
        File.WriteAllText(Application.persistentDataPath + "/InventorySave.json", json);
    }
    public static InventoryData Inventory_Load()
    {

        string path = Application.persistentDataPath + "/InventorySave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);
            return data; 
        }
        else
        {
            return null;
        }
    }

    public static void Statue_Save(int[] indexs)
    {
        Debug.Log(indexs.Length);
        StatueData data = new StatueData();
        foreach (var index in indexs)
        {

            data.ints.Add(index);
        }

        string json = JsonUtility.ToJson(data);
        Debug.Log("Statue_Save : " + json);
        File.WriteAllText(Application.persistentDataPath + "/StatueSave.json", json);
        Debug.Log("Application.persistentDataPath : " + Application.persistentDataPath);
    }
    public static StatueData Statue_Load()
    {

        string path = Application.persistentDataPath + "/StatueSave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            StatueData data = JsonUtility.FromJson<StatueData>(json);

            return data;
        }
        else
        {
            //Debug.Log("Statue_Save : null");
            return null;
        }

    }
    public static void Money_Save(int money)
    {
        MoneyData moneyData;
        moneyData.money = money;
        string json = JsonUtility.ToJson(moneyData);
        Debug.Log("Money_Save : " + json);
        File.WriteAllText(Application.persistentDataPath + "/MoneySave.json", json);
        Debug.Log("Application.persistentDataPath : " + Application.persistentDataPath);
    }

    public static MoneyData Money_Load()
    {
        string path = Application.persistentDataPath + "/MoneySave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MoneyData data = JsonUtility.FromJson<MoneyData>(json);
            return data;
        }
        else
        {
            return default(MoneyData);
        }
    }
    public static void Volum_Save(float bgm,float sfx)
    {
        VolumData volumData;
        volumData.bgm = bgm;
        volumData.sfx = sfx;
        string json = JsonUtility.ToJson(volumData);
        Debug.Log("Volum_Save : " + json);
        File.WriteAllText(Application.persistentDataPath + "/VolumSave.json", json);
        Debug.Log("Application.persistentDataPath : " + Application.persistentDataPath);
    }

    public static VolumData Volum_Load()
    {
        string path = Application.persistentDataPath + "/VolumSave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            VolumData data = JsonUtility.FromJson<VolumData>(json);
            return data;
        }
        else
        {
            return VolumData.Default;
        }
    }
}
