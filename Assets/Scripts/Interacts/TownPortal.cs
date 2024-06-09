using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownPortal : PortalController
{
    [SerializeField] SpriteRenderer fadeSprite;
    private new void Awake()
    {
        base.Awake();

        colliderRadius = 10f;
    }
    public override void interact()
    {
        fadeSprite.gameObject.SetActive(true);
        Save?.Invoke();
        StartCoroutine(FadeOut("Town"));
    }
    IEnumerator FadeOut(string sceneName)
    {
        Color black = Color.black;
        black.a = 0f;
        while (true)
        {
            black.a += Time.deltaTime;
            fadeSprite.color = black;
            if (black.a >= 1f)
            {
                LoadingManager.LoadScene(sceneName);
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
