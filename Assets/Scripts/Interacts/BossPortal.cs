using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BossPortal : PortalController
{

    [SerializeField] TMP_Text bossTxt;
    [SerializeField] Button goLeftBtn;
    [SerializeField] Button killTheBossBtn;
    [SerializeField] Button goRightBtn;
    [SerializeField] BossImageSO[] bossImages;
    BossImageSO currentImage;
    int currentIndex = 0;

    private new void Awake()
    {
        base.Awake();
        colliderRadius = 9f;

        ChangeBossImage(currentIndex);
        ChangeInteractable();
        goLeftBtn.onClick.AddListener(() =>
        {
            ChangeBossImage(-1);

        });
        goRightBtn.onClick.AddListener(() =>
        {
            ChangeBossImage(1);
        });

        killTheBossBtn.onClick.AddListener(() =>
        {

            KillTheBoss(currentIndex);
        });
    }



    void ChangeBossImage(int index)
    {
        currentImage = bossImages[currentIndex + index];
        bossTxt.text = currentImage.name;
        killTheBossBtn.image.sprite = currentImage.BossImage;
        currentIndex += index;

        ChangeInteractable();
    }
    void ChangeInteractable()
    {
        goLeftBtn.interactable = currentIndex <= 0 ? false : true;
        goRightBtn.interactable = currentIndex >= bossImages.Length - 1 ? false : true;
    }

    void KillTheBoss(int index)
    {
        Save?.Invoke();
        LoadingManager.LoadScene("Boss" + index);
    }

}
