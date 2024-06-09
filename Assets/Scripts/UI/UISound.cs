using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISound : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void OnEnable()
    {
        LoadSoundData();
    }
    void Update()
    {
        SoundManager.instance.BGMVolume(bgmSlider.value);
        SoundManager.instance.SFXVolume(sfxSlider.value);
    }
    private void OnDisable()
    {
        JsonSaveLoader.Volum_Save(bgmSlider.value, sfxSlider.value);
    }
    public void LoadSoundData()
    {
        VolumData volumData = JsonSaveLoader.Volum_Load();
        bgmSlider.value = volumData.bgm;
        sfxSlider.value = volumData.sfx;
        SoundManager.instance.BGMVolume(bgmSlider.value);
        SoundManager.instance.SFXVolume(sfxSlider.value);
    }
}
