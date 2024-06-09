using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cinemachine;

public class AddressableManager : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject[] ojbects;
    [SerializeField] AssetReferenceGameObject virCamera;
    [SerializeField] AssetReferenceGameObject player;

    //[SerializeField] AssetReferenceT<AudioClip> soundBGM;
    //[SerializeField] AssetReferenceSprite flagSprite;

    //[SerializeField] private GameObject BGMObj;
    //[SerializeField] private Image flagImg;
    [SerializeField] Vector3 playerStartPos;
    private List<GameObject> gameObjects = new List<GameObject>();

    void Awake()
    {
        Init_InstantiateAsync();
    }

    public void Init_InstantiateAsync()
    {
        for (int i = 0; i < ojbects.Length; i++)
        {
            ojbects[i].InstantiateAsync().Completed += (obj) =>
            {
                gameObjects.Add(obj.Result);

            };
        }
        player.LoadAssetAsync().Completed += (obj) =>
        {
            gameObjects.Add(obj.Result);
            Instantiate(obj.Result, playerStartPos, Quaternion.identity);
        };
        //boss.LoadAssetAsync().Completed += (obj) =>
        //{
        //    gameObjects.Add(obj.Result);
        //    Instantiate(obj.Result);
        //};
        virCamera.LoadAssetAsync().Completed += (obj) =>
        {
            gameObjects.Add(obj.Result);
            GameObject virCamera = Instantiate(obj.Result);
            virCamera.GetComponent<CinemachineVirtualCamera>().Follow = FindObjectOfType<PlayerController>().transform;
        };
        //gameObjects[3].GetComponent<CinemachineVirtualCamera>().Follow = FindObjectOfType<PlayerController>().transform;

        //soundBGM.LoadAssetAsync<AudioClip>().Completed += (clip) =>
        //{
        //    var bgmSound = BGMObj.GetComponent<AudioSource>();
        //    bgmSound.clip = clip.Result;
        //    bgmSound.loop = true;
        //    bgmSound.Play();
        //};

        //flagSprite.LoadAssetAsync<Sprite>().Completed += (img) =>
        //{
        //    var image = flagImg.GetComponent<Image>();
        //    image.sprite = img.Result;
        //};
    }

    //public void Button_Release()
    //{
    //    //soundBGM.ReleaseAsset();
    //    //flagSprite.ReleaseAsset();

    //    if (gameObjects.Count == 0)
    //        return;
    //    var index = gameObjects.Count - 1;
    //    Addressables.ReleaseInstance(gameObjects[index]);
    //    gameObjects.RemoveAt(index);
    //}
}
