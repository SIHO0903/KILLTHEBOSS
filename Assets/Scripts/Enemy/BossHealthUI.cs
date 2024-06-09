using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BossHealthUI : MonoBehaviour
{
    [SerializeField] TMP_Text bossNameTxt;
    [SerializeField] Slider bossHealthBar;
    [SerializeField] TMP_Text bossHealthTxt;

    float timer;
    float lerpTime = 3f;
    float fontSize;
    float yPos;

    RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = bossNameTxt.GetComponent<RectTransform>();

    }
    public IEnumerator BossText(string bossName,IEnumerator Spawn)
    {

        for (int i = 0; i < bossName.Length; i++)
        {
            Color randColor = Random.ColorHSV();
            string randHexCode = ColorUtility.ToHtmlStringRGBA(randColor);
            bossNameTxt.text += $"<color=#{randHexCode}>" + bossName[i].ToString() + "</color>";
            yield return new WaitForSeconds(0.15f);
        }

        while (timer < lerpTime)
        {
            timer += Time.deltaTime;
            fontSize = Mathf.Lerp(220, 60, timer / lerpTime);
            yPos = Mathf.Lerp(-500, 0, timer / lerpTime);
            bossNameTxt.fontSize = fontSize;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPos);
            yield return new WaitForEndOfFrame();
        }

        yield return Spawn;
    }
    public void BossNameTxt(string name)
    {
        bossNameTxt.text = name;
    }
    public void HealthUI(float curHealth,float maxHealth)
    {
        bossHealthBar.value = curHealth / maxHealth;
        bossHealthTxt.text = curHealth + " / " + maxHealth;
    }

}
