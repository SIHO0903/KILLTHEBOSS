using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;


public class DamageText : MonoBehaviour
{
    float moveSpeed = 2f;
    float disappearSpeed;

    TextMeshPro text;
    Color alpha;
    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
    }
    private void OnEnable()
    {
        alpha.a = 1;
        disappearSpeed = 0f;
        StartCoroutine(DamageTxtPopUp());
    }

    IEnumerator DamageTxtPopUp()
    {
        while (gameObject.activeSelf)
        {
            disappearSpeed += Time.deltaTime;
            transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
            //transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);


            text.color = alpha;
            yield return new WaitForEndOfFrame();
            if (disappearSpeed >= 1f)
                gameObject.SetActive(false);
        }

    }
    void SetUp(float damage, Color color,float fontSize)
    {
        text.text = damage.ToString();
        alpha = color;
        text.fontSize = fontSize;
        text.alignment = TextAlignmentOptions.Center;
        text.fontStyle = FontStyles.Bold;
    }

    public static DamageText Create(Vector3 pos, float damage, Color color,float fontSize)
    {
        GameObject damageTextObj = PoolManager.instance.Get(PoolEnum.Player, 1, pos, Quaternion.identity);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        damageTextObj.GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-1f, 1f) + pos.x, pos.y, -2f);
        //Transform objTransform = damageTextObj.transform;
        //objTransform.localPosition = new Vector3(Random.Range(-1f, 1f) + pos.x, pos.y, -2f);

        damageText.SetUp(damage, color, fontSize);

        return damageText;
    }


}
