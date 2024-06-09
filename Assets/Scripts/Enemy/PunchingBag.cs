using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//»÷µå¹é
public class PunchingBag : EnemyEntity
{
    //[SerializeField] Transform damageTxtTransform;
    Vector3 damageTxtPos;

    public override void Start()
    {
        enemyStat.curHealth = enemyStat.maxHealth;
        damageTxtPos = transform.position + Vector3.up * 1.5f;
    }

    public override void OnDamaged(float damage, Color color,float fontSize)
    {
        enemyStat.curHealth -= damage;
        DamageText.Create(damageTxtPos, damage, color, fontSize);
    }

    public override IEnumerator Die()
    {
        yield return null;
    }

    public override IEnumerator Spawn()
    {
        throw new System.NotImplementedException();
    }
}
