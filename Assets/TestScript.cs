using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] float test1;
    [SerializeField] float test2;
    [SerializeField] float test3;

    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        test1 += Time.deltaTime*test2;
        text.text = (Mathf.Sin(test1)+1)/test3 + "";
    }
}
