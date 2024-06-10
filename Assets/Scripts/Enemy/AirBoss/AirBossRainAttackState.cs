using UnityEngine;

public class AirBossRainAttackState : BaseBossState<AirBoss>
{
    float featherSpeed = 11f;
    float setSpeed = 0.03f;
    float ShootTimer;
    int shotCount;
    Vector3 ShootPos = new(0, 6.5f, 0);
    float randomNoFeather;
    Vector3 projectileStartPos = new Vector3(-17f, 11f, 0f);

    Rigidbody2D rigid;
    public override void EnterState(AirBoss boss, Transform player)
    {
        rigid = boss.GetComponent<Rigidbody2D>();
        shotCount = 0;
        ShootTimer = 0;
    }
    public override void UpdateState(AirBoss boss, Transform player)
    {
        //시작시 위치조정
        if (Vector3.Distance(boss.transform.position, ShootPos) > 0.5f)
        {
            rigid.velocity = Vector3.zero;
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, ShootPos, setSpeed);
        }
        else
        {           
            ShootTimer += Time.deltaTime;
            if (ShootTimer > 1.5f)
            {
                ShootTimer = 0f;
                shotCount++;
                if (shotCount > 3)
                {
                    boss.PatternSwitch();
                    return;
                }
                SoundManager.instance.PlaySound(SoundType.Boss2_RainAttack);
                //깃털 회피지역 랜덤난수발생
                randomNoFeather = Random.Range(0, 30);
                for (int j = 0; j < 30; j++)
                {
                    //깃털위치 변환후 발사
                    Vector3 projectilePos;
                    if(j > randomNoFeather)
                        projectilePos = new Vector3(projectileStartPos.x + j+4, projectileStartPos.y, projectileStartPos.z);
                    else
                        projectilePos = new Vector3(projectileStartPos.x + j  , projectileStartPos.y, projectileStartPos.z);
                    GameObject feather = PoolManager.instance.Get(PoolEnum.Enemy, 1, projectilePos, Quaternion.identity);
                    feather.GetComponent<EnemyProjectile>().damage = boss.enemyStat.projectileDamage;
                    Rigidbody2D rigid = feather.GetComponent<Rigidbody2D>();
                    rigid.gravityScale = 1.3f;
                    rigid.velocity = feather.GetComponent<EnemyProjectile>().projectileLookAt(projectilePos + Vector3.down)  * featherSpeed;
                }

            }
        }
    }
}

