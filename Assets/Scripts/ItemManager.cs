using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public ItemSO[] itemSOs;

    private Dictionary<int, ItemSO> itemDictionarByItemID;
    private Dictionary<string, ItemSO> itemDictionaryByItemName;
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
        InitializeItemDictionary();
    }
    private void InitializeItemDictionary()
    {
        itemDictionarByItemID = new Dictionary<int, ItemSO>();
        itemDictionaryByItemName = new Dictionary<string, ItemSO>();
        foreach (var item in itemSOs)
        {
            itemDictionarByItemID.Add(item.ID, item);
            itemDictionaryByItemName.Add(item.name, item);

        }
    }

    public ItemSO GetItemByItemID(int itemID)
    {
        if (itemDictionarByItemID.TryGetValue(itemID, out ItemSO item))
        {
            return item;
        }
        else
        {
            Debug.Log("아이디에 해당하는 아이템이 없습니다");
            return null;
        }
    }
    public ItemSO GetItemByItemName(string itemID)
    {
        if (itemDictionaryByItemName.TryGetValue(itemID, out ItemSO item))
        {
            return item;
        }
        else
        {
            Debug.Log("아이디에 해당하는 아이템이 없습니다");
            return null;
        }
    }

}
