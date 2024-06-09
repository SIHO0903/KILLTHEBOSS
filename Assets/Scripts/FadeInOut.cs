using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    TMP_Text text;
    Color color;
    void Start()
    {
        text = GetComponent<TMP_Text>();
        color = Color.white;

        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {

        while (true)
        {
            color.a -= Time.deltaTime;
            text.color = color;
            if (color.a < 0)
            {
                SceneManager.LoadScene("DownLoading");
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
