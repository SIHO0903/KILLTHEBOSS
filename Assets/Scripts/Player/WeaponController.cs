using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Transform weaponSlot;
    public WeaponSO currentWeapon;
    SpriteRenderer weaponSpriteRenderer;
    Sprite previousSprite;
    float curAttackRate;
    float totalAttackRate;

    float itmeDamage;
    public float ItemDamage
    {
        get
        {
            itmeDamage = GetComponent<PlayerController>().TotalDamage;
            return itmeDamage;
        }
    }
    public float AddDamage { get; set; }
    [Header("Swing")]
    Quaternion startSwing;


    float startpos = -50;
    float swingSpeed = 0.9f;

    Action Attack;
    PolygonCollider2D weaponPolygonCollider2D;
    private void Start()
    {
        weaponPolygonCollider2D = weaponSlot.GetComponentInChildren<PolygonCollider2D>(true);
        weaponSpriteRenderer = weaponSlot.GetComponentInChildren<SpriteRenderer>(true);
        ResetTransform();
        curAttackRate = 0;
    }

    private void Update()
    {
        curAttackRate += Time.deltaTime;
        if (Input.GetKey(KeyCode.Mouse0) && currentWeapon != null)
        {
            if (curAttackRate >= 1 / AttackRateCal() && !weaponSlot.gameObject.activeSelf)
                weaponSlot.gameObject.SetActive(true);
            Attack?.Invoke();

        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ResetTransform();
            weaponSlot.gameObject.SetActive(false);
            curAttackRate = 0;
        }

        UpdateCollider();
    }
    void UpdateCollider()
    {
        if (weaponSpriteRenderer.sprite != previousSprite)
        {
            if (weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
            {

                List<Vector2> physicsShape = new List<Vector2>();
                weaponSpriteRenderer.sprite.GetPhysicsShape(0, physicsShape);

                weaponPolygonCollider2D.pathCount = 1;
                weaponPolygonCollider2D.SetPath(0, physicsShape);
            }
            previousSprite = weaponSpriteRenderer.sprite;
        }

    }
    void MagicAttack() // 원거리 공격
    {

        InventoryController inventory = GetComponent<InventoryController>();
        if (curAttackRate >= 1 / AttackRateCal())
        {
            if(inventory.AmmoItemCheck().Quantity > 0)
            {
                GameObject prefab = PoolManager.instance.Get(PoolEnum.Player, 0, weaponSlot.position, Quaternion.identity);
                //GameObject prefab = AddressablePoolManager.instance.Get(PoolEnum.Player, 0, weaponSlot.position, Quaternion.identity);
                prefab.GetComponent<Projectile>().GetWeaponSO(this);
                inventory.UseAmmo(inventory.AmmoItemCheck());
                SoundManager.instance.PlaySound(SoundType.MagicAttack);
            }


            
            curAttackRate = 0;
        }
    }
    private void MeleeAttack()
    {

        weaponSlot.Rotate(-transform.localScale.x * Vector3.forward, AttackRateCal() * swingSpeed);

        if (curAttackRate >= 1 / AttackRateCal())
        {
            SoundManager.instance.PlaySound(SoundType.MeleeAttack);
            ResetTransform();
            curAttackRate = 0;
        }
    }

    public void ChangeWeapon(UIInventory inventory, int itemIndex)
    {
        currentWeapon = (WeaponSO)inventory.ItemSelected(itemIndex);
        if (currentWeapon == null)
            return;
        switch (currentWeapon.weaponType)
        {
            case WeaponSO.WeaponType.Melee:

                weaponPolygonCollider2D.enabled = true;
                Attack = null;
                Attack = MeleeAttack;

                break;
            case WeaponSO.WeaponType.Magic:
                weaponPolygonCollider2D.enabled = false;
                Attack = null;
                Attack = MagicAttack;
                break;
        }
        ResetTransform();
    }

    private void ResetTransform()
    {
        if (currentWeapon != null)
            weaponSpriteRenderer.sprite = currentWeapon.itemImage;
        startSwing = Quaternion.Euler(new Vector3(0, 0, -transform.localScale.x* startpos));
        weaponSlot.rotation = startSwing;

    }

    private float AttackRateCal()
    {
        if (currentWeapon != null)
            totalAttackRate = currentWeapon.attackRate * (1f);
        return totalAttackRate;
    }
    public void ADD_Damage(float amount)
    {
        AddDamage += amount;
    }

    public float HitDamage()
    {
        float finalDamage;
        finalDamage = AddDamage + currentWeapon.damage + ItemDamage;
        if (currentWeapon.criticalChance > UnityEngine.Random.Range(0, 101))
        {
            currentWeapon.currentDamageTxtColor = currentWeapon.criticalDamageTxtColor;
            currentWeapon.currentFontSize = currentWeapon.criticalDamageFontSize;
            finalDamage *= 2;
        }
        else
        {
            currentWeapon.currentDamageTxtColor = currentWeapon.damageTxtColor;
            currentWeapon.currentFontSize = currentWeapon.fontSize;
        }
        return finalDamage;
    }
}
