using System.Collections.Generic;
using UnityEngine;

public class BossDropItem : MonoBehaviour
{
    //[Header("GroundBoss")]
    //public ItemSO[] groundBossItems;

    //[Header("AirBoss")]
    //public ItemSO[] airBossItems;

    //public List<(string name, int quantity)> itemNameAndQuantities;
    //public int Length
    //{
    //    get
    //    {
    //        return itemNameAndQuantities.Count;
    //    }
    //}

    //public BossDropItem()
    //{
    //    itemNameAndQuantities = new List<(string name, int quantity)>();
    //    itemNameAndQuantities = ItemDropList();

    //}

    //public List<(string name, int quantity)> ItemDropList()
    //{
    //    itemNameAndQuantities.Add(("PumpKin", 12));
    //    itemNameAndQuantities.Add(("Money", 30000));

    //    return itemNameAndQuantities;
    //}

    //public void DropItems(Transform transform)
    //{
    //    List<GameObject> itemObjects = new List<GameObject>();
    //    Vector3 startPos = transform.position + Vector3.up * 0.5f;
    //    for (int i = 0; i < itemNameAndQuantities.Count; i++)
    //    {
    //        itemObjects.Add(PoolManager.instance.Get(PoolEnum.Player, 2, startPos, Quaternion.identity));
    //        Item item = itemObjects[i].GetComponent<Item>();
    //        item.item = ItemManager.instance.GetItemByItemName(itemNameAndQuantities[i].name);
    //        item.GetVar(itemNameAndQuantities[i].quantity, itemNameAndQuantities.Count, i);
    //        itemObjects[i].SetActive(true);
    //    }
    //}


}

public interface IDropItem
{
    List<(string name, int quantity)> ItemDropList();
    void DropItems(Transform transform);
}



public class GroundBossDropItem : IDropItem
{
    public List<(string name, int quantity)> itemNameAndQuantities;
    public int Length
    {
        get
        {
            return itemNameAndQuantities.Count;
        }
    }
    public GroundBossDropItem()
    {
        itemNameAndQuantities = new List<(string name, int quantity)>();
        itemNameAndQuantities = ItemDropList();

    }

    public List<(string name, int quantity)> ItemDropList()
    {
        itemNameAndQuantities.Add(("PumpKin", 30));
        itemNameAndQuantities.Add(("Money", 50000));

        return itemNameAndQuantities;
    }

    public void DropItems(Transform transform)
    {
        List<GameObject> itemObjects = new List<GameObject>();
        Vector3 startPos = transform.position + Vector3.up * 0.5f;
        for (int i = 0; i < itemNameAndQuantities.Count; i++)
        {
            itemObjects.Add(PoolManager.instance.Get(PoolEnum.Player, 2, startPos, Quaternion.identity));
            Item item = itemObjects[i].GetComponent<Item>();
            item.item = ItemManager.instance.GetItemByItemName(itemNameAndQuantities[i].name);
            item.GetVar(itemNameAndQuantities[i].quantity, itemNameAndQuantities.Count, i);
            itemObjects[i].SetActive(true);
        }
    }

}

public class AirBossDropItem : IDropItem
{
    public List<(string name, int quantity)> itemNameAndQuantities = new List<(string name, int quantity)>();
    public AirBossDropItem()
    {
        itemNameAndQuantities = new List<(string name, int quantity)>();
        itemNameAndQuantities = ItemDropList();
    }

    public List<(string name, int quantity)> ItemDropList()
    {
        itemNameAndQuantities.Add(("Bear", 30));
        itemNameAndQuantities.Add(("Money", 80000));

        return itemNameAndQuantities;
    }

    public void DropItems(Transform transform)
    {
        List<GameObject> itemObjects = new List<GameObject>();
        Vector3 startPos = transform.position + Vector3.up * 0.5f;
        for (int i = 0; i < itemNameAndQuantities.Count; i++)
        {
            itemObjects.Add(PoolManager.instance.Get(PoolEnum.Player, 2, startPos, Quaternion.identity));

            Item item = itemObjects[i].GetComponent<Item>();
            item.item = ItemManager.instance.GetItemByItemName(itemNameAndQuantities[i].name);
            item.Reset();
            item.GetVar(itemNameAndQuantities[i].quantity, itemNameAndQuantities.Count, i);
            itemObjects[i].SetActive(true);
        }
    }

}
