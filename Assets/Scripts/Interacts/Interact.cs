using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{
    public void interact();
}
public class Interact : MonoBehaviour, IInteractable
{
    bool isIn = false;
    public bool Toggle { get; private set; } = false;
    public event Action CheckShop;
    [SerializeField] 
    protected GameObject shopUI;
    GameObject interactUI;
    protected InventoryController inventory;

    protected float colliderRadius = 3.5f;
    private void Start()
    {
        interactUI = shopUI.transform.parent.gameObject;
        CircleCollider2D circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
        circleCollider2D.radius = colliderRadius;
        circleCollider2D.isTrigger = true;

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inventory = collision.GetComponent<InventoryController>();
            isIn = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isIn = false;
            if (shopUI.activeSelf)
            {
                objectToggle(false);
                Debug.Log("OnTriggerExit2D : ²¨Áü");

            }
        }

    }

    public virtual void interact()
    {
        objectToggle(isIn);
    }
    void objectToggle(bool val)
    {
        shopUI.SetActive(val);
        interactUI.SetActive(val);
        Toggle = val;
        CheckShop?.Invoke();

    }
}
