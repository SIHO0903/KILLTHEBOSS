using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDragPanel : MonoBehaviour
{
    UIItem uiItem;
    Canvas canvas;
    private void Awake()
    {
        uiItem = GetComponentInChildren<UIItem>(true);
        canvas = transform.parent.GetComponent<Canvas>();
        Toggle(false);
    }

    public void SetData(Sprite sprite, int quantity)
    {
        uiItem.SetData(sprite, quantity);
    }

    public UIItem GetData()
    {
        return uiItem;
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, Input.mousePosition,
            canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }
}
