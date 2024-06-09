using UnityEngine;

public class GroundBossVolcanoAttackState : BaseBossState<GroundBoss>
{
    float rockSpeed = 20f;
    float setSpeed= 0.03f;
    float ShootTimer;
    int shotCount;
    Vector3 ShootPos = new(0.2f, -7.5f, 0);

    public override void EnterState(GroundBoss boss, Transform player)
    {
        shotCount = 0;
    }

    public override void UpdateState(GroundBoss boss, Transform player)
    {

        if (Vector3.Distance(boss.transform.position, ShootPos) > 0.5f)
        {
            boss.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, ShootPos, setSpeed);
            Debug.Log("움직임 조정중");
        }
        else
        {
            ShootTimer += Time.deltaTime;

            if (ShootTimer > 0.5f)
            {
                SoundManager.instance.PlaySound(SoundType.Boss1_Volcano);
                boss.CameraShaking();
                if (shotCount >= 3)
                {
                    boss.SwitchState(boss.NormalAttackState);
                    boss.GimmickTimer = 0;
                }
                for (int j = 0; j < 8; j++)
                {
                    //GameObject rock = MyUtils.Instansiate(boss.enemyStat.objects[0], boss.transform.position, Quaternion.identity, boss.prefabBox);
                    GameObject rock = PoolManager.instance.Get(PoolEnum.Enemy, 0, boss.transform.position, Quaternion.identity);
                    rock.GetComponent<EnemyProjectile>().damage = boss.enemyStat.projectileDamage;
                    float randomAngle = Random.Range(-40f, 40f);
                    Vector3 projectileDir = Quaternion.Euler(0, 0, randomAngle) * Vector2.up;
                    rock.GetComponent<Rigidbody2D>().gravityScale = 1.3f;
                    rock.GetComponent<Rigidbody2D>().velocity = projectileDir * rockSpeed;

                }
                shotCount++;
                ShootTimer = 0f;              
            }
        }
    }
}
