using System.Collections;
using UnityEngine;

public class GroundBoss : EnemyEntity
{


    float floor_Distance = 1.95f;
    Vector3 floor_Dir = Vector3.down;
    Vector2 floor_Size = new Vector2(1f, 0.3f);
    Vector2 lastVelocity;

    GameObject townPortal;

    BaseBossState<GroundBoss> currentState;
    public GroundBossNormalAttackState NormalAttackState = new GroundBossNormalAttackState();
    public GroundBossBounceAttackState BounceAttackState = new GroundBossBounceAttackState();
    public GroundBossRushAttackState RushAttackState = new GroundBossRushAttackState();
    public GroundBossShootAttackState ShootAttackState = new GroundBossShootAttackState();
    public GroundBossVolcanoAttackState VolcanoAttackState = new GroundBossVolcanoAttackState();
    GroundBossDropItem groundBossDropItem;

    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        groundBossDropItem = new GroundBossDropItem();
    }
    public override void Start()
    {
        base.Start();

        enemyStat.curHealth = enemyStat.maxHealth;
    }
    private void Update()
    {
        TimeChecker();
        currentState?.UpdateState(this,player.transform);

    }
    void TimeChecker()
    {
        GimmickTimer += Time.deltaTime;
        IsGround = MyUtils.WhatFloor(transform.position, floor_Distance, floor_Dir, floor_Size, "Ground");
        LookDir = player.transform.position.x - transform.position.x > 0 ? 1 : -1;
        if (IsGround)
            sprite.flipX = LookDir > 0 ? true : false;
    }
    public void SwitchState(BaseBossState<GroundBoss> state)
    {
        currentState = state;
    }
    public void PatternSwitch()
    {
        int pattern;
        if (IsPhase2())
            pattern = Random.Range(1,5);
        else
            pattern = Random.Range(1,3);
        switch (pattern)
        {
            case 1:
                SwitchState(ShootAttackState);
                break;
            case 2:
                SwitchState(RushAttackState);
                break;
            case 3:
                SwitchState(BounceAttackState);
                break;
            case 4:
                SwitchState(VolcanoAttackState);
                break;
        }
        currentState.EnterState(this, player.transform);

    }

    public override IEnumerator Spawn()
    {
        Debug.Log("½ºÆù");
        PatternSwitch();
        yield return null;
    }
    public override IEnumerator Die()
    {
        currentState = null;
        Color color = Color.white;
        while (true)
        {
            color.a -= Time.deltaTime*0.5f;
            sprite.color = color;
            if (color.a < 0)
            {
                groundBossDropItem.DropItems(transform);
                townPortal = GameObject.FindGameObjectWithTag("Portal");
                townPortal.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.transform.CompareTag("Wall"))
        {
            if (rigid.velocity.y > 0)
                lastVelocity = rigid.velocity;

            Vector3 reflectDir = Vector3.Reflect(lastVelocity.normalized, collision.transform.position);
            rigid.velocity = reflectDir.normalized * Mathf.Max(0f, 1f);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (floor_Distance * floor_Dir), floor_Size);
    }

}
