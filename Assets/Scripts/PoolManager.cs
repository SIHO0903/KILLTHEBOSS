using System.Collections.Generic;
using UnityEngine;

public enum PoolEnum
{
    Player,
    Enemy,
    Item,
}

[System.Serializable]
public class PoolType 
{
    public PoolEnum poolType;
    public GameObject[] prefabs;
    [HideInInspector]public List<GameObject>[] pools;
}

public class PoolManager : MonoBehaviour
{
    [SerializeField] List<PoolType> objectDatas = new List<PoolType>();
    public static PoolManager instance;


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

        for (int dataIdx = 0; dataIdx < objectDatas.Count; dataIdx++)
        {
            PoolType poolData = objectDatas[dataIdx];
            poolData.pools = new List<GameObject>[poolData.prefabs.Length];


            for (int i = 0; i < poolData.pools.Length; i++)
            {
                poolData.pools[i] = new List<GameObject>();
            }

            objectDatas[dataIdx] = poolData;
        }

    }
    public GameObject Get(PoolEnum prefabTypes, int index,Vector3 startPos, Quaternion quaternion, Transform transform = null)
    {
        GameObject select = null;

        if (transform == null)
            transform = this.transform;

        //�ش� �������� ����Ʈ���� ��Ȱ��ȭ�Ȱ��� �մٸ� Ȱ��ȭ
        foreach (GameObject item in objectDatas[(int)prefabTypes].pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.transform.position = startPos;
                select.SetActive(true);
                break;
            }
        }
        //�������� ���� Ȱ��ȭ�����Ͻ� ������ ����Ʈ�� �߰�
        if (select == null)
        {
            select= Instantiate(objectDatas[(int)prefabTypes].prefabs[index], startPos, quaternion, transform);
            objectDatas[(int)prefabTypes].pools[index].Add(select);
        }

        return select;
    }



}
