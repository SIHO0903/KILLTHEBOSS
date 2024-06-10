using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class PoolType1
{
    public PoolEnum poolType;
    public AssetReferenceGameObject[] prefabs;
    [HideInInspector] public List<GameObject>[] pools;
}

public class AddressablePoolManager : MonoBehaviour
{
    [SerializeField] List<PoolType1> objectDatas = new List<PoolType1>();
    public static AddressablePoolManager instance;


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
            PoolType1 poolData = objectDatas[dataIdx];
            poolData.pools = new List<GameObject>[poolData.prefabs.Length];


            for (int i = 0; i < poolData.pools.Length; i++)
            {
                poolData.pools[i] = new List<GameObject>();
            }

            objectDatas[dataIdx] = poolData;
        }

    }
    public GameObject Get(PoolEnum prefabTypes, int index, Vector3 startPos, Quaternion quaternion, Transform transform = null)
    {
        GameObject select = null;

        if (transform == null)
            transform = this.transform;

        //해당 프리팹의 리스트에서 비활성화된것이 잇다면 활성화
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

        if (select == null)
        {
            AssetReferenceGameObject assetReference = objectDatas[(int)prefabTypes].prefabs[index];
            select = InstantiateAddressable(assetReference, startPos, quaternion, transform, objectDatas[(int)prefabTypes].pools[index]);
        }

        return select;
    }

    private GameObject InstantiateAddressable(AssetReferenceGameObject assetReference, Vector3 position, Quaternion rotation, Transform parent, List<GameObject> pool)
    {
        GameObject newObject = null;
        assetReference.InstantiateAsync(position, rotation, parent).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                newObject = handle.Result;
                pool.Add(newObject);
            }
            else
            {
                Debug.LogError($"Failed to instantiate addressable prefab: {assetReference.AssetGUID}");
            }
        };

        return newObject;
    }
}
