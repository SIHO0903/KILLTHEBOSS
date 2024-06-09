using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PortalController : Interact
{
    bool isPortalActive;
    protected Animator anim;
    protected Action Save;
    public void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player"))
        {
            Save = collision.GetComponent<InventoryController>().SaveData;
            SoundManager.instance.PlaySound(SoundType.Portal);
            isPortalActive = !isPortalActive;
            anim.SetBool("IsPortalActive", isPortalActive);
        }
    }
    private new void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Player"))
        {
            isPortalActive = !isPortalActive;
            anim.SetBool("IsPortalActive", isPortalActive);
        }
    }
}
