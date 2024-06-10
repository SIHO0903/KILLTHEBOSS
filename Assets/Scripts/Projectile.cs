using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 dirPos;
    Rigidbody2D rigid;
    float timer;
    public ProjectileSO projectileData;
    [HideInInspector] public WeaponController weapon;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        timer = 0;
        dirPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dirPos.y, dirPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
        StartCoroutine(SetActiveFalseDelay());
    }
    IEnumerator SetActiveFalseDelay()
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > 2f)
                gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
    }
    private void FixedUpdate()
    {
        rigid.velocity = dirPos.normalized * projectileData.speed * Time.fixedDeltaTime;
    }
    private void OnDisable()
    {
        //StopCoroutine(SetActiveFalseDelay());
        StopAllCoroutines();
    }

    public void GetWeaponSO(WeaponController weapon)
    {
        this.weapon = weapon;
    }
    public float HitDamage()
    {
        float finalDamage;
        finalDamage = weapon.currentWeapon.damage + projectileData.damage + weapon.AddDamage + weapon.ItemDamage;
        if (weapon.currentWeapon.criticalChance > Random.Range(0, 101))
        {
            weapon.currentWeapon.currentDamageTxtColor = weapon.currentWeapon.criticalDamageTxtColor;
            weapon.currentWeapon.currentFontSize = weapon.currentWeapon.criticalDamageFontSize;
            finalDamage *= 2;
        }
        else
        {
            weapon.currentWeapon.currentDamageTxtColor = weapon.currentWeapon.damageTxtColor;
            weapon.currentWeapon.currentFontSize = weapon.currentWeapon.fontSize;
        }

        return finalDamage;
    }
}
