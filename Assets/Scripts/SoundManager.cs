using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public enum SoundType
{
    MagicAttack,
    MeleeAttack,
    Dash,
    Fall,
    Jump,
    PlayerGetHit,
    PlayerDie,
    UsePotion,
    Portal,
    Upgrade,
    Boss1_Volcano,
    Boss2_RainAttack,
    BuyItem,
    SellItem,
    GetItem,
    InventoryItemClick,
    NotEnoughMoney,
    SoundOptionClick,

}

public enum BGMType
{
    Town,

}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Tooltip("ContextMenu의 InitializeSoundLists를 실행시켜 리스트이름을 초기화하세요")]
    //public SounList[] soundList;
    //public SounList[] bgmList;

    public SounList[] soundList;
    public SounList[] bgmList;
    public static SoundManager instance;
    private AudioSource sfxAudioSource;
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioMixer mixer;
    UISound uiSound;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        sfxAudioSource = GetComponent<AudioSource>();
        uiSound = GetComponentInChildren<UISound>(true);

        LoadAllSounds();
    }
    private void Start()
    {
        uiSound.LoadSoundData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(uiSound.gameObject.activeSelf)
                uiSound.gameObject.SetActive(false);
            else
                uiSound.gameObject.SetActive(true);
        }

    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgmList.Length; i++)
        {
            if (arg0.name == bgmList[i].name)
            {         
                PlayBGM(BGMType.Town);
            }
            else
            {
                bgmAudioSource.Stop();
            }
        }
    }

    public void PlaySound(SoundType sound, float volume = 1)
    {

        sfxAudioSource.PlayOneShot(soundList[(int)sound].Sound, volume);
        sfxAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    public void PlayBGM(BGMType sound,float volume = 1)
    {
        bgmAudioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        bgmAudioSource.clip = bgmList[(int)sound].Sound;
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = volume;
        bgmAudioSource.Play();
    }

    public void BGMVolume(float val)
    {
        mixer.SetFloat("mixerBGMVolume", Mathf.Log10(val) * 20f);
    }
    public void SFXVolume(float val)
    {
        mixer.SetFloat("mixerSFXVolume", Mathf.Log10(val) * 20f);
    }

    [ContextMenu("Initialize Sound Lists")]
    private void InitializeSoundLists()
    {
        string[] soundNames = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, soundNames.Length);
        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = soundNames[i];
        }

        string[] bgmNames = Enum.GetNames(typeof(BGMType));
        Array.Resize(ref bgmList, bgmNames.Length);
        for (int i = 0; i < bgmList.Length; i++)
        {
            bgmList[i].name = bgmNames[i];
        }
    }

    private void LoadAllSounds()
    {
        foreach (SounList sound in soundList)
        {
            sound.LoadSound();
        }

        foreach (SounList bgm in bgmList)
        {
            bgm.LoadSound();
        }
    }

}

[System.Serializable]
public class SounList
{
    public AudioClip Sound { get => sound; }
    [HideInInspector] public string name;
    [SerializeField] private AssetReferenceT<AudioClip> soundReference;
    private AudioClip sound;

    public void LoadSound()
    {
        if (sound == null)
        {
            soundReference.LoadAssetAsync().Completed += handle =>
            {
                sound = handle.Result;
            };
        }
    }

}
