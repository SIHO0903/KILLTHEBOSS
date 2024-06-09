using UnityEngine;

public class AirBossRushAttackState : BaseBossState<AirBoss>
{
    float rushPower = 20f;
    float rushAttackAlertTimer;
    float rushAttackAlertTimer_Multiply = 3f;

    float dashTimer;
    

    Vector3 startPos;
    Vector3 endPos;
    Vector3 targetDir;
    Gradient gradient;
    GradientAlphaKey[] alphaKeys;
    LineRenderer lineRenderer;
    Rigidbody2D rigid;
    public override void EnterState(AirBoss boss, Transform player)
    {
        lineRenderer = boss.GetComponent<LineRenderer>();
        rigid = boss.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector3.zero;
        dashTimer = 0;
        startPos = boss.transform.position;
        targetDir = (player.position - startPos).normalized;
        endPos = startPos + targetDir * 40f; 

        gradient = new Gradient();
        alphaKeys = new GradientAlphaKey[2];
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 3;
        lineRenderer.endWidth = 3;
        rushAttackAlertTimer = 0;


        //isEnteringOrbit = true;
        //phase2Timer = 0;
        //angle = Random.Range(0f, 360f);



    }

    public override void UpdateState(AirBoss boss, Transform player)
    {
        if (rushAttackAlertTimer < 5f)
        {
            rushAttackAlertTimer += Time.deltaTime * rushAttackAlertTimer_Multiply;


            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);

            alphaKeys[0].alpha = (Mathf.Sin(rushAttackAlertTimer * rushAttackAlertTimer_Multiply) + 1) / 4;
            alphaKeys[1].alpha = (Mathf.Sin(rushAttackAlertTimer * rushAttackAlertTimer_Multiply) + 1) / 4;

            gradient.SetKeys(lineRenderer.colorGradient.colorKeys, alphaKeys);
            lineRenderer.colorGradient = gradient;

            targetDir = endPos - boss.transform.position;
            boss.Flip(targetDir);
        }
        else
        {
            dashTimer += Time.deltaTime;

            if (dashTimer < 1.1f)
            {
                
                rigid.velocity = targetDir.normalized * rushPower;
            }
            else
            {
                rigid.velocity = Vector2.zero;
                lineRenderer.positionCount = 0;
                boss.GimmickTimer = 0;
                boss.PatternSwitch();
            }




        }
    }
}

