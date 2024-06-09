using UnityEngine;

public class AirBossShootState : BaseBossState<AirBoss>
{
    float featherSpeed = 15f;
    int curBulletCount;
    float shotTimer;


    Vector3 setPos;
    float distance;
    Rigidbody2D rigid;
    public override void EnterState(AirBoss boss, Transform player)
    {
        rigid = boss.GetComponent<Rigidbody2D>();

        setPos = new Vector3(10f+Random.Range(-3f,3f), 4.5f + Random.Range(-1.5f, 1.5f), 0f);

        curBulletCount = 0;
        shotTimer = 0;
    }

    public override void UpdateState(AirBoss boss, Transform player)
    {
        distance = Vector3.Distance(setPos, boss.transform.position);
        if (distance > 0.3f)
        {
            rigid.velocity = (setPos - boss.transform.position).normalized * boss.enemyStat.moveSpeed;
        }
        else
            rigid.velocity = Vector3.zero;

        int bulletCount = boss.Phase2Check() ? 5 : 3;
        shotTimer += Time.deltaTime;
        if (shotTimer > 0.3f && curBulletCount < bulletCount)
        {
            boss.Flip(player.transform.position-boss.transform.position);
            curBulletCount++;
            shotTimer = 0;
            GameObject feather = PoolManager.instance.Get(PoolEnum.Enemy, 1, boss.transform.position, Quaternion.identity);
            feather.GetComponent<EnemyProjectile>().damage = boss.enemyStat.projectileDamage;
            Rigidbody2D rigid = feather.GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.velocity = feather.GetComponent<EnemyProjectile>().projectileLookAt(player.position) * featherSpeed;
        }
        else if(curBulletCount >= bulletCount)
        {
            boss.PatternSwitch();
        }
    }
}

