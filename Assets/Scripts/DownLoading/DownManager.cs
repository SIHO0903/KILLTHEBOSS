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

    private long patchSize;  //�� ��ġũ��
    private Dictionary<string,long> patchMap = new Dictionary<string,long>(); //�󺧺� ��ġũ�� ����

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

    //��ġ�� ũ�� üũ
    IEnumerator CheckUpdateFiles()
    {
        patchSize = default;

        foreach (var label in labels) //�� ��ġ�� ũ��
        {
            //�󺧿� �ش��ϴ� ������ �ٿ��� handle�� ����
            AsyncOperationHandle<long> handle = Addressables.GetDownloadSizeAsync(label.labelString);

            //handle�� ���� �ٿ��Ҷ����� ��ٸ�
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

    //��ġ�����ȯ
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

        foreach (var label in labels) //�� ��ġ�� ũ��
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

    //���� �ٿ�ε�
    IEnumerator DownLoadLabel(string label)
    {
        patchMap.Add(label, 0);

        var handle = Addressables.DownloadDependenciesAsync(label,true); //���ϴٿ��� �ڵ鿡 ����

        while (!handle.IsDone)
        {
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;     
            yield return new WaitForEndOfFrame();
        }

        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    //�ٿ�ε� �����̵��
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
