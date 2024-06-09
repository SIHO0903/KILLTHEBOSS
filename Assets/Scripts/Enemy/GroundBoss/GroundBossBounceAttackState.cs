using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBossBounceAttackState : BaseBossState<GroundBoss>
{
    float jumpDistance = 1.2f;
    float jumpHeight = 4f;
    float jumpPower = 28f;
    int curJump;
    int jumpCount = 5;
    Rigidbody2D rigid;
    public override void EnterState(GroundBoss boss,Transform player)
    {

        rigid = boss.GetComponent<Rigidbody2D>();
        curJump = 0;
        rigid.gravityScale = 4f;
    }

    public override void UpdateState(GroundBoss boss, Transform player)
    {


        if (boss.IsGround && rigid.velocity.y==0)
        {
            Vector3 positionCheckter = new Vector2(boss.transform.position.x + (jumpDistance * boss.LookDir), jumpHeight);
            rigid.velocity = (positionCheckter - boss.transform.position).normalized * jumpPower;
            curJump++;
            if (curJump > jumpCount)
            {
                boss.GimmickTimer = 0;
                boss.SwitchState(boss.NormalAttackState);
            }
        }

    }
}
