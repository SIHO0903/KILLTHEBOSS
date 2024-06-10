using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class DownManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject waitMessage;
    public GameObject downMessage;
    public Slider downSlider;
    public TMP_Text sizeInfoText;
    public TMP_Text downValText;

    [Header("Label")]
    [SerializeField] List<AssetLabelReference> labels = new List<AssetLabelReference>();

    private long patchSize;  //총 패치크기
    private Dictionary<string,long> patchMap = new Dictionary<string,long>(); //라벨별 패치크기 저장

    void Start()
    { 
        waitMessage.SetActive(true);
        downMessage.SetActive(false);

        StartCoroutine(InitAddressable());
        StartCoroutine(CheckUpdateFiles());

    }

    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    #region Chek Down

    //패치할 크기 체크
    IEnumerator CheckUpdateFiles()
    {
        patchSize = default;

        foreach (var label in labels) //총 패치할 크기
        {
            //라벨에 해당하는 번들을 다운해 handle에 저장
            AsyncOperationHandle<long> handle = Addressables.GetDownloadSizeAsync(label.labelString);

            //handle을 전부 다운할때까지 기다림
            yield return handle;

            patchSize += handle.Result;
        }

        if(patchSize > decimal.Zero)
        {
            waitMessage.SetActive(false);
            downMessage.SetActive(true);

            sizeInfoText.text = GetFileSize(patchSize);
        }
        else
        {
            downValText.text = " 100 % ";
            downSlider.value = 1f;

            yield return new WaitForSeconds(2f);
            LoadingManager.LoadScene("Town");
        }
    }

    //패치사이즈변환
    private string GetFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        if(byteCnt >= 1073741824.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1073741824.0) + " GB";
        }
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1048576.0) + " MB";
        }
        else if (byteCnt >= 1024.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1024.0) + " KB";
        }
        else if(byteCnt > 0 && byteCnt < 1024.0)
        {
            size = byteCnt.ToString() + " Bytes";
        }

        return size;
    }
    #endregion

    #region DownLoad
    public void Button_DownLoad()
    {
        StartCoroutine(PatchFiles());
    }
    IEnumerator PatchFiles()
    {

        foreach (var label in labels) //총 패치할 크기
        {
            var handle = Addressables.GetDownloadSizeAsync(label.labelString);

            yield return handle;

            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label.labelString));
            }
        }

        yield return CheckDownLoad();
    }

    //파일 다운로드
    IEnumerator DownLoadLabel(string label)
    {
        patchMap.Add(label, 0);

        var handle = Addressables.DownloadDependenciesAsync(label,true); //파일다운후 핸들에 대입

        while (!handle.IsDone)
        {
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;     
            yield return new WaitForEndOfFrame();
        }

        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    //다운로드 슬라이드바
    IEnumerator CheckDownLoad()
    {
        var total = 0f;
        downValText.text = "0 %";

        while (true)
        {
            total += patchMap.Sum(tmp => tmp.Value);

            downSlider.value = total / patchSize;
            downValText.text = (int)(downSlider.value * 100) + " %";

            if(total == patchSize)
            {
                LoadingManager.LoadScene("Town");
                break;
            }

            total = 0;
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}
