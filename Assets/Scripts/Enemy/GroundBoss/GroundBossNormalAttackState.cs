using UnityEngine;



public class GroundBossNormalAttackState : BaseBossState<GroundBoss>
{
    float baseAttackTimer;

    float jumpDistance = 1f;
    float jumpHeight = 8f;
    float jumpPower = 10f;
    public override void EnterState(GroundBoss boss, Transform player)
    {
        baseAttackTimer = 0;
        boss.GetComponent<Rigidbody2D>().gravityScale = 1f;
        if (boss.GimmickTimer > 5f)
        {
            boss.PatternSwitch();
            return;
        }
        Vector3 positionCheckter = new Vector2(boss.transform.position.x + (jumpDistance * boss.LookDir), jumpHeight);
        boss.GetComponent<Rigidbody2D>().velocity = (positionCheckter - boss.transform.position).normalized * jumpPower;

    }

    public override void UpdateState(GroundBoss boss, Transform player)
    {
        baseAttackTimer += Time.deltaTime;
        if (baseAttackTimer > 3f && boss.IsGround)
        {
            EnterState(boss,player);
            baseAttackTimer = 0;
        }
    }

}
