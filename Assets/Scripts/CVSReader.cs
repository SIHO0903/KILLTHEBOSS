using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CVSReader : MonoBehaviour
{
    [SerializeField] TMP_Text tipText;
    static string tipCSVPath = "/Editor/CSVs/TipTextData.csv";
    string[] tipList;
    private void Awake()
    {
        ReadCVS();
    }
    private void Start()
    {
        PrintTip();
    }
    void PrintTip()
    {
        int randnum;
        randnum = Random.Range(0,tipList.Length);
        tipText.text = tipList[randnum];
    }

    void ReadCVS()
    {
        tipList = File.ReadAllLines(Application.dataPath + tipCSVPath); 
    }
}
