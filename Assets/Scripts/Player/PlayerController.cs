using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    [Header("Move")]
    [SerializeField] 
    float       moveSpeed;
    const float defaultGravityScale = 5f;
    float       dirX;
    float       dirY;
    float canMove;
    bool canHit;

    [Header("Jump")]
    [SerializeField] float jumpPower;
    [SerializeField] float jumpTime;
    bool    isJump;
    float   curJumpTime;
    bool    isLadder;

    bool canDash;
    bool isDashClick;
    float dashPower =12f;
    float dashCoolTime;

    [Header("Stats")]
    float currentHealth;
    [SerializeField] float maxHealth;
    float totalHealth;
    public float TotalHealth { 
        get
        {
            return totalHealth;
        }
        private set
        {
            totalHealth = value;
            Health(0);
        } 
    }
    float healthRecoverySpeed = 0.3f;
    bool isHealthRecoveyActive;

    public float currentDefense;
    float totalDefense;
    public float TotalDefense
    {
        get
        {
            return totalDefense;
        }
        set
        {
            totalDefense = value;
            Defense();
        }
    }

    float currentDamage;
    public float TotalDamage { get; set; }

    [SerializeField] Slider healthBar;
    [SerializeField] TMP_Text defenseTxt;
    TMP_Text healthTxt;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer playerSprite;
    SpriteRenderer weaponSprite;
    BoxCollider2D collider2D;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        anim  = GetComponent<Animator>();
        healthTxt = healthBar.GetComponentInChildren<TMP_Text>();
        totalHealth = maxHealth;
        currentHealth = TotalHealth;
        playerSprite = GetComponent<SpriteRenderer>();
        weaponSprite = GetComponentInChildren<SpriteRenderer>();
        anim.SetTrigger("DeathEnded");
        collider2D.enabled =true;
    }
    void Update()
    {
        Movecontroller();
        Interactive();
    }
    private void FixedUpdate()
    {
        Move();
    }
    #region Move
    private void Movecontroller()
    {
        dirX = Input.GetAxis("Horizontal");
        dirY = Input.GetAxisRaw("Vertical");
        anim.SetFloat("Speed", Mathf.Abs(dirX));
        anim.SetBool("Grounded", MyUtils.WhatFloor(transform.position,"Ground", "Platform"));
        anim.SetFloat("Yvelocity", rigid.velocity.y);
        TryJump();
        TryDash();
        GetDown();

        LadderCheck();
        Ladderout();

        Fall();
        Flip();
    }

    private void Ladderout()
    {
        if (!MyUtils.WhatFloor(transform.position, "Ladder"))
        {
            rigid.gravityScale = defaultGravityScale;
            isLadder = false;
        }
    }

    private void Fall()
    {
        if (Input.GetButtonUp("Jump"))
            isJump = false;
    }

    private void LadderCheck()
    {
        if (dirY != 0 && MyUtils.WhatFloor(transform.position, "Ladder"))
        {
            rigid.gravityScale = 0f;
            isLadder = true;
        }
    }

    private void TryJump()
    {
        if (Input.GetButtonDown("Jump") && MyUtils.WhatFloor(transform.position, "Ground", "Platform"))
        {
            curJumpTime = 0;
            isJump = true;
        }
    }

    void TryDash()
    {
        dashCoolTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCoolTime > 1.5f)
        {
            isDashClick = true;
            dashCoolTime = 0;
        }
    }

    private void GetDown()
    {
        if (Input.GetKey(KeyCode.S) && MyUtils.WhatFloor(transform.position, "Platform"))
        {
            StartCoroutine(GetDownPlatform());
        }
    }

    private void Move()
    {

        if(canMove == 0) rigid.velocity = new Vector2(dirX * moveSpeed, rigid.velocity.y);


        if (isJump)
        {
            curJumpTime += Time.fixedDeltaTime;

            if (curJumpTime > jumpTime) isJump = false;
            float t = jumpTime - curJumpTime;
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * (1 - t));
        }

        if (isLadder)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, dirY * moveSpeed);
        }

        //대쉬
        if (canDash && isDashClick)
        {
            Debug.Log("대쉬");
            //SoundManager.instance.PlaySound(SoundType.Dash);
            rigid.AddForce(Vector2.right * transform.localScale.x * dashPower, ForceMode2D.Impulse);
            StartCoroutine(MoveCoroutine());
            isDashClick = false;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * 1f), Vector2.one/2f);
    }
    IEnumerator GetDownPlatform()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Platform");
        RaycastHit2D platform = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, layerMask);
        if (platform.collider == null)
        {
            yield break;
        }
        platform.transform.GetComponent<PlatformEffector2D>().colliderMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
        yield return new WaitForSeconds(0.25f);
        platform.transform.GetComponent<PlatformEffector2D>().colliderMask = (1 << LayerMask.NameToLayer("Player"));

    }
    private void Flip()
    {
        if (dirX > 0) transform.localScale = new Vector3(1, 1, 1);
        if (dirX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
    #endregion
    void Interactive()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);


            if (hit.transform != null && hit.transform.TryGetComponent(out IInteractable interactObj))
            {
                Debug.Log(hit.transform.name);
                interactObj.interact();
            }

        }

    }

    public void Health(float amount)
    {
        currentHealth += amount;
        if (currentHealth >= TotalHealth)
        {
            currentHealth = TotalHealth;
        }
        if(!isHealthRecoveyActive)
            StartCoroutine(HealthRecovery());
        UpdateHealthUI();
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        float timer =0;
        if (currentHealth <= 0)
        {
            Debug.Log("사망");
            SoundManager.instance.PlaySound(SoundType.PlayerDie);
            GetComponent<InventoryController>().SaveData();
            canMove = 1;
            anim.SetTrigger("Death");
            collider2D.enabled = false;
            while (true)
            {
                timer += Time.deltaTime;
                if(timer > 2f)
                {
                    LoadingManager.LoadScene("Town");
                    break;
                }
                yield return null;
            }

        }
    }

    void UpdateHealthUI()
    {
        healthBar.value = currentHealth / TotalHealth;
        healthTxt.text = $"{currentHealth:N0} / {TotalHealth}";
    }
    public void HealthUpgrade(float amount)
    {
        TotalHealth += amount;
        maxHealth += amount;
        Health(amount);
    }

    IEnumerator HealthRecovery()
    {
        isHealthRecoveyActive = true;
        while (currentHealth <= TotalHealth)
        {
            currentHealth += Time.deltaTime * healthRecoverySpeed;
            UpdateHealthUI();
            yield return new WaitForEndOfFrame();
        }
        isHealthRecoveyActive = false;
    }

    public void Defense()
    {
        defenseTxt.text = TotalDefense + "";
    }
    public void UpgradeDefenfse(float amount)
    {
        currentDefense += amount;
        TotalDefense += amount;
    }

    public void AddEffect(EffectSO effect)
    {
        TotalDefense += effect.DefenseAmount;

        if (currentHealth >= TotalHealth)
        {
            TotalHealth += effect.AddHealthAmount;
            Health(effect.AddHealthAmount);
        }
        else
        {

            TotalHealth += effect.AddHealthAmount;
        }
        TotalDamage += effect.DamageAmount;
        canDash = canDash || effect.canDash;
    }
    
    private void ResetStats()
    {
        TotalDefense = currentDefense;
        TotalHealth = maxHealth;
 
        TotalDamage = currentDamage;
        canDash = false;
    }

    public void CurrentEquipEffects(List<UIItem> equips)
    {
        ResetStats();
        for (int i = 0; i < equips.Count; i++)
        {
            if (equips[i].item == null)
            {
                continue;
            }
            else
            {
                foreach (var effect in equips[i].item.effects)
                {
                    AddEffect(effect);
                }
                
            }
        }
        //Debug.Log("TotalDefense : " + TotalDefense);
        //Debug.Log("totalHealth : " + TotalHealth);
        //Debug.Log("totalDamage : " + totalDamage);
    }
    public void ADDHealthRecoverySpeed(float amount)
    {
        healthRecoverySpeed += amount;
    }


    IEnumerator MoveCoroutine()
    {
        while (canMove < 0.15f)
        {
            canMove += Time.deltaTime;
            yield return null;
        }
        canMove = 0;
    }
    IEnumerator HitEffect()
    {
        canHit = true;
        int count = 0;
        WaitForSeconds wait = new WaitForSeconds(0.07f);
        Color color = new Color(1, 1, 1, 0.3f);
        while(count < 3)
        {
            playerSprite.color = color;
            weaponSprite.color = color;
            yield return wait;
            playerSprite.color = Color.white;
            weaponSprite.color = Color.white;
            yield return wait;
            count++;
        }

        canHit = false;
    }
    void GetDamage(Vector2 dir, float damage)
    {
        damage -= TotalDefense;
        rigid.velocity = Vector3.zero;
        dir.y = 0; dir = dir.normalized; dir.y = 1f; dir *= 7f;
        Vector3 damageTxtPos = transform.position + Vector3.up * 1.5f;
        DamageText.Create(damageTxtPos, damage, Color.red,5f);
        rigid.AddForce(dir, ForceMode2D.Impulse);
        //SoundManager.instance.PlaySound(SoundType.PlayerGetHit);
        Health(-damage);
        StartCoroutine(MoveCoroutine());
        StartCoroutine(HitEffect());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !canHit)
        {
            Vector2 dir = transform.position - collision.transform.position;
            float damage = collision.gameObject.GetComponent<EnemyEntity>().enemyStat.contactDamage;
            GetDamage(dir, damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") && !canHit)
        {
            Vector2 dir = transform.position - collision.transform.position;
            float damage = collision.gameObject.GetComponent<EnemyProjectile>().damage;
            GetDamage(dir, damage);
        }
        if (collision.gameObject.CompareTag("EnemyAOE") && !canHit)
        {
            Vector2 dir = Vector2.up;
            float damage = collision.gameObject.GetComponent<EnemyAOE>().damage;
            GetDamage(dir, damage);
        }
    }

}

