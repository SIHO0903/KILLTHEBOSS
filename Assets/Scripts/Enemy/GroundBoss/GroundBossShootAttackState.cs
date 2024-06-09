using UnityEngine;


public class GroundBossShootAttackState : BaseBossState<GroundBoss>
{
    // �߻�ü�� ������
    // ������������ ���ν�� 
    // �ƴҶ��� ����� ���
    float rockSpeed = 10f;


    public override void EnterState(GroundBoss boss,Transform player)
    {
        int bulletCount = boss.Phase2Check() ? 5 : 3;
        int bulletYOffset = PlayerYPos(boss.transform, player, bulletCount);
        float bulletXOffset = boss.LookDir;
        for (int i = 0; i < bulletCount; i++)
        {
            //GameObject rock = MyUtils.Instansiate(boss.enemyStat.objects[0], boss.transform.position, Quaternion.identity, boss.prefabBox);
            GameObject rock = PoolManager.instance.Get(PoolEnum.Enemy, 0, boss.transform.position, Quaternion.identity);
            rock.GetComponent<EnemyProjectile>().damage = boss.enemyStat.projectileDamage;
            Vector3 dir = (player.transform.position - boss.transform.position).normalized;
            Vector3 projectileDir = Quaternion.Euler(0, 0, (i + bulletYOffset) * bulletXOffset * 15f) * dir;
            Rigidbody2D rigid = rock.GetComponent<Rigidbody2D>();
            rigid.gravityScale = 0f;
            rigid.velocity = projectileDir * rockSpeed;
        }

        boss.SwitchState(boss.NormalAttackState);
        boss.GimmickTimer = 0;
    }

    /// <summary>
    /// �÷��̾ �������� ���������� ��� �߻�ü�� �÷��̾����� ���󰡰�
    /// �÷��̾ ������ ���� y�࿡ ������ ù��° �߻�ü�� �÷��̾����� ���󰡰�
    /// </summary>
    /// <returns></returns>
    int PlayerYPos(Transform boss,Transform player,int bulletCount)
    {
        float bossYPos = boss.position.y;
        float playerYPos = player.position.y;
        float yDistance = bossYPos - playerYPos;
        float finalYDistance = Mathf.Sqrt(yDistance * yDistance);

        if(finalYDistance < 1f)
        {
            Debug.Log("���� y��");
            return 0;
        }
        else
        {
            Debug.Log("�÷��̾ �� ���� ����");
            if (bulletCount == 3)
                return -1;
            else
                return -2;
        }


    }

    public override void UpdateState(GroundBoss boss, Transform player)
    {

    }

}
