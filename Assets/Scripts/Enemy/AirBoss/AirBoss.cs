using System.Collections;
using UnityEngine;

public class AirBoss : EnemyEntity
{
    BaseBossState<AirBoss> currentState;
    public AirBossShootState ShootAttackState = new AirBossShootState();
    public AirBossRushAttackState RushAttackState = new AirBossRushAttackState();
    public AirBossAOEAttackState AOEAttackState = new AirBossAOEAttackState();
    public AirBossRainAttackState RainAttackState = new AirBossRainAttackState();
    AirBossDropItem airBossDropItem;
    GameObject townPortal;
    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        airBossDropItem = new AirBossDropItem();

    }
    public override void Start()
    {
        base.Start();
        enemyStat.curHealth = enemyStat.maxHealth;
    }

    void Update()
    {
        currentState?.UpdateState(this, player.transform);
    }

    public void Flip(Vector3 dir)
    {
        LookDir = dir.x > 0 ? 1 : -1;
        sprite.flipX = LookDir > 0 ? true : false;
    }
    public override IEnumerator Spawn()
    {
        Debug.Log("½ºÆù");
        PatternSwitch();
        yield return null;
    }
    public void PatternSwitch()
    {
        int pattern;
        if (IsPhase2())
            pattern = Random.Range(1, 5);
        else
            pattern = Random.Range(1, 3);
        switch (pattern)
        {
            case 1:
                SwitchState(ShootAttackState);
                break;
            case 2:
                SwitchState(RushAttackState);
                break;
            case 3:
                SwitchState(RainAttackState);
                break;
            case 4:
                SwitchState(AOEAttackState);
                break;
        }
        currentState?.EnterState(this, player.transform);

    }

    public void SwitchState(BaseBossState<AirBoss> state)
    {
        currentState = state;
    }
    public override IEnumerator Die()
    {
        currentState = null;
        Color color = Color.white;
        while (true)
        {
            color.a -= Time.deltaTime * 0.5f;
            sprite.color = color;
            if (color.a < 0)
            {
                airBossDropItem.DropItems(transform);
                townPortal = GameObject.FindGameObjectWithTag("Portal");
                townPortal.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
}
