using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//아이템 칸 오브젝트에 붙어있는 스크립트
public class UIItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField] Image itemImage;
    [SerializeField] TMP_Text quantityTxt;
    [SerializeField] TMP_Text UsingTxt;
    [SerializeField] Image borderImage;
    public event Action<UIItem> PointerRightClick,PointerShiftRightClick, BeginDrag, EndDrag, Drop, PointerExit;
    public event Func<UIItem, UIDescription> PointerMove;

    public bool empty = true;

    public ItemSO item;
    public int Quantity { get; set; }
    private void Awake()
    {
        ResetData();
    }
    //아이템칸의 데이터초기화
    public void ResetData()
    {
        itemImage.enabled = false;
        quantityTxt.gameObject.SetActive(false);
        item = null;
        empty = true;
    }
    public void SetData(Sprite itemImage, int quantity, ItemSO item=null)
    {

        this.itemImage.sprite = itemImage;
        quantityTxt.text = quantity + "";
        this.Quantity = quantity;
        empty = false;
        this.item = item;
        this.itemImage.enabled = true;
        quantityTxt.gameObject.SetActive(true);
    }
    public void SetData(ItemSO item,int quantity)
    {
        SetData(item.itemImage, quantity, item);
    }
    public void SellItem()
    {
        GameManager.instance.Money += item.SellCost;
        Updatequantity(-1);
    }
    //아이템 획득또는 구매시 수량 업데이트
    public void Updatequantity(int quantity)
    {
        quantity = int.Parse(quantityTxt.text) + quantity;
        this.Quantity = quantity;
        if (quantity <= 0)
        {
            ResetData();
        }
        quantityTxt.text = quantity + "";

    }
    public void InitUsingTxt(int num)
    {
        itemImage.enabled = false;
        UsingTxt.gameObject.SetActive(true);
        UsingTxt.text = num + "";
    }
    public void ToggleQuantityTxt(bool val)
    {
        quantityTxt.gameObject.SetActive(val);
    }
    public void ItemSelected(bool val)
    {
        if (val)
        {
            borderImage.color = Color.yellow;
        }
        else
        {
            borderImage.color = new Color(1, 1, 1, 0.2f);
        }

    } 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (empty) return;
        SoundManager.instance.PlaySound(SoundType.InventoryItemClick);
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                PointerShiftRightClick?.Invoke(this);
            else
                PointerRightClick?.Invoke(this);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty) return;
        BeginDrag?.Invoke(this);
    }
    public void OnDrag(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        Drop?.Invoke(this);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit?.Invoke(this);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        if (empty) return;
        UIDescription description = PointerMove?.Invoke(this);
        if (description != null)
            description.transform.position = eventData.position;
    }

}
