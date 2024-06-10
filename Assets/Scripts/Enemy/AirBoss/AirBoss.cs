using System.Collections;
using UnityEngine;

public class AirBoss : EnemyEntity
{
    //스테이트패턴
    BaseBossState<AirBoss> currentState;
    public AirBossShootState ShootAttackState = new AirBossShootState();
    public AirBossRushAttackState RushAttackState = new AirBossRushAttackState();
    public AirBossAOEAttackState AOEAttackState = new AirBossAOEAttackState();
    public AirBossRainAttackState RainAttackState = new AirBossRainAttackState();  
    AirBossDropItem airBossDropItem;//드랍아이템클래스
    GameObject townPortal; //클리어후 포탈 
    public override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        airBossDropItem = new AirBossDropItem();

    }
    //패턴업데이트
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
        Debug.Log("스폰");
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
    //보스사망시 아이템드랍 및 포탈활성화
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
