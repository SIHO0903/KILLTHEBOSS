using System.Collections;
using UnityEngine;



public class GroundBossRushAttackState : BaseBossState<GroundBoss>
{
    float rushPower = 20f;
    float rushAttackAlertTimer;
    float rushAttackAlertTimer_Multiply=3f;

    Gradient gradient;
    GradientAlphaKey[] alphaKeys;
    LineRenderer lineRenderer;
    Rigidbody2D rigid;
    public override void EnterState(GroundBoss boss, Transform player)
    {

        lineRenderer = boss.GetComponent<LineRenderer>();
        rigid = boss.GetComponent<Rigidbody2D>();
        gradient = new Gradient();
        alphaKeys = new GradientAlphaKey[2];
        lineRenderer.positionCount = 2;
        rushAttackAlertTimer = 0;

    }

    public override void UpdateState(GroundBoss boss, Transform player)
    {
        rushAttackAlertTimer += Time.deltaTime * rushAttackAlertTimer_Multiply;

        Vector3 startPos = new Vector3(boss.transform.position.x, -7.5f, 0);
        Vector3 endPos = new(boss.LookDir * 20f, -7.5f, 0);
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        alphaKeys[0].alpha = (Mathf.Sin(rushAttackAlertTimer * rushAttackAlertTimer_Multiply) + 1) / 4;
        alphaKeys[1].alpha = (Mathf.Sin(rushAttackAlertTimer * rushAttackAlertTimer_Multiply) + 1) / 4;

        gradient.SetKeys(lineRenderer.colorGradient.colorKeys, alphaKeys);
        lineRenderer.colorGradient = gradient;
    
        if (rushAttackAlertTimer > 3f)
        {

            Vector3 targetDir = endPos - boss.transform.position;

            rigid.AddForce(targetDir.normalized * rushPower, ForceMode2D.Impulse);

            lineRenderer.positionCount = 0;
            boss.GimmickTimer = 0;
            boss.SwitchState(boss.NormalAttackState);
        }
    }

    


}
