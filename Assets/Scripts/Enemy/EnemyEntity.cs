using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyEntity : MonoBehaviour
{
    public float LookDir { get; set; }
    public bool IsGround { get; set; }
    public float GimmickTimer { get; set; }
    public EnemyStatsSO enemyStat;
    bool isOnHit;
    int hitTimerCount;
    Material glowMat;
    WaitForSeconds colorBlinkTime;
    BossHealthUI healthUI;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rigid;
    protected PlayerController player;
    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        
        sprite = GetComponent<SpriteRenderer>();
        healthUI = GetComponentInChildren<BossHealthUI>();
        glowMat = GetComponent<SpriteRenderer>().material;
        colorBlinkTime = new WaitForSeconds(0.1f);
    }
    public virtual void Start()
    {
        healthUI.HealthUI(enemyStat.curHealth, enemyStat.maxHealth);
        StartCoroutine(healthUI.BossText(enemyStat.name, Spawn()));
    }
    public virtual void OnDamaged(float damage, Color color,float fontSize)
    {
        enemyStat.curHealth -= damage;
        DamageText.Create(transform.position, damage, color, fontSize);
        healthUI.HealthUI(enemyStat.curHealth, enemyStat.maxHealth);
        hitTimerCount = 0;
        BossSpriteChange();

        if (enemyStat.curHealth <= 0)
        {
            enemyStat.curHealth = 0;
            Debug.Log("보스사망");
            StartCoroutine(Die());
        }
        else
        {
            if (!isOnHit) StartCoroutine(DamagedEffect());
        }
    }

    void BossSpriteChange()
    {
        if (enemyStat.curHealth < enemyStat.maxHealth / 2)
        {
            Debug.Log("페이즈2 컬러변환");
            Color phase2Color = new Color(1, 0.5f, 0.5f, 1);
            sprite.color = phase2Color;
        }

    }

    IEnumerator DamagedEffect()
    {
        isOnHit = true;
        while (hitTimerCount < 3)
        {
            Color hitColor = new Color(1, 1, 1, 0);
            glowMat.SetColor("_GlowColor", hitColor);
            yield return colorBlinkTime;
            hitColor = new Color(0, 0, 0, 0);
            glowMat.SetColor("_GlowColor", hitColor);
            yield return colorBlinkTime;
            hitTimerCount++;
        }
        isOnHit = false;
    }

    public void CameraShaking()
    {
        StartCoroutine(CameraShake.ShakeCoroutine(0.1f));
    }
    public bool IsPhase2()
    {
        bool phase2;

        if (enemyStat.curHealth > enemyStat.maxHealth / 2)
            phase2 = false;
        else
            phase2 = true;

        return phase2;
    }
    public abstract IEnumerator Spawn();

    public abstract IEnumerator Die();

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Melee"))
        {
            WeaponController weapon = collision.GetComponentInParent<WeaponController>();

            OnDamaged(weapon.HitDamage(),weapon.currentWeapon.currentDamageTxtColor,weapon.currentWeapon.currentFontSize);

        }
        else if (collision.CompareTag("Magic"))
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            OnDamaged(projectile.HitDamage(), projectile.weapon.currentWeapon.currentDamageTxtColor, projectile.weapon.currentWeapon.currentFontSize);
            collision.gameObject.SetActive(false);

        }
    }

}
