using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO item;
    [field: SerializeField] public int Quantity { get; set; } = 1;
    int popUpLength;
    int popUpIndex;


    float popUpPower = 8f;
    float rotateSpeed = 200f;
    float floatingTimer;
    float floatingSpeed=5;
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = item.itemImage;

    }
    private void OnEnable()
    {
        rigid.gravityScale = 1;
        StartCoroutine(PopUpItem(popUpLength, popUpIndex));
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * .5f), Vector2.one / 2f);
    }

    public void Reset()
    {
        sprite.sprite = item.itemImage;
        Quantity = 0;
    }

    public IEnumerator PopUpItem(int popUpLength,int popUpIndex)
    {
        float midAngle = Mathf.FloorToInt(popUpLength/2);
        Vector3 direction = Quaternion.Euler(0, 0, 6f * (-midAngle + popUpIndex)) * Vector3.up;
        rigid.AddForce(direction * popUpPower, ForceMode2D.Impulse);


        while (true)
        {
            if(MyUtils.WhatFloor(transform.position, 0.5f, Vector2.down, Vector2.one / 2, "Ground"))
            {
                rigid.gravityScale = 0;
                transform.rotation = Quaternion.identity;
                rigid.velocity = Vector2.zero;

                break;
            }
            else
            {
                transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            }
            yield return new WaitForEndOfFrameUnit();

        }

        while (true)
        {
            if(gameObject.activeSelf)
            {
                floatingTimer += Time.deltaTime * floatingSpeed;
                rigid.velocity = Vector2.up * (Mathf.Sin(floatingTimer)*2) / 4;
            }
            yield return new WaitForEndOfFrame();
        }

    }

    public void GetVar(int quantity,int popUpLength,int popUpIndex)
    {
        Quantity = quantity;
        this.popUpLength = popUpLength;
        this.popUpIndex = popUpIndex;
    }
}
