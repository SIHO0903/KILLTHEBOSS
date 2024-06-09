using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    [SerializeField] Slider loadingBar;
    void Start()
    {
        StartCoroutine(StartLoadingScene());

    }


    IEnumerator StartLoadingScene()
    {

        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        //로드할 다음씬을 수동으로 전환
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, op.progress, timer); //슬라이더를 부드럽게 업데이트

                if (loadingBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);

                if(loadingBar.value == 1f)
                {
                    yield return new WaitForSeconds(2f);
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }


}
