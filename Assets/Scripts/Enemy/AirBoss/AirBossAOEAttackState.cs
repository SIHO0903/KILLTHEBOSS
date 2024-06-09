using UnityEngine;

public class AirBossAOEAttackState : BaseBossState<AirBoss>
{
    float rushAttackAlertTimer;
    float rushAttackAlertTimer2;
    float AOETimer;
    float rushAttackAlertTimer_Multiply = 3f;

    Vector3 setPos;
    float setSpeed = 0.03f;

    //XPos = -18~18 , YPos = -10 ~ 10(-6,.5,7) 6.5
    Vector3[] startPos = new Vector3[3];
    Vector3[] endPos = new Vector3[3];
    int[] array = { 0, 1, 2 };
    int alertIndex = 0;
    int AOEIndex = 0;

    Gradient gradient;
    GradientAlphaKey[] alphaKeys;
    LineRenderer lineRenderer;
    Rigidbody2D rigid;
    public override void EnterState(AirBoss boss, Transform player)
    {
        lineRenderer = boss.GetComponent<LineRenderer>();
        rigid = boss.GetComponent<Rigidbody2D>();
        setPos = new Vector3(10f + Random.Range(-3f, 3f), 4.5f + Random.Range(-1.5f, 1.5f), 0f);
        gradient = new Gradient();
        alphaKeys = new GradientAlphaKey[2];
        lineRenderer.positionCount = 2;
        rushAttackAlertTimer = -3f;
        rushAttackAlertTimer2 = 0;
        lineRenderer.startWidth = 6.5f;
        lineRenderer.endWidth = 6.5f;
        array = ShuffleArray(array);
        alertIndex = 0;
        AOEIndex = 0;
        startPos[0] = new Vector3(-18f, -6f, 0f);
        startPos[1] = new Vector3(-18f, 0.5f, 0f);
        startPos[2] = new Vector3(-18f, 7f, 0f);
        endPos[0] = new Vector3(18f, -6f, 0f);
        endPos[1] = new Vector3(18f, 0.5f, 0f);
        endPos[2] = new Vector3(18f, 7f, 0f);
    }

    public override void UpdateState(AirBoss boss, Transform player)
    {
        if (Vector3.Distance(boss.transform.position, setPos) > 0.5f)
        {
            rigid.velocity = Vector3.zero;
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, setPos, setSpeed);
            Debug.Log("움직임 조정중");
        }
        else
        {
            rushAttackAlertTimer += Time.deltaTime * rushAttackAlertTimer_Multiply;
            if (rushAttackAlertTimer >= 3f && alertIndex < 3)
            {

                lineRenderer.SetPosition(0, startPos[array[alertIndex]]);
                lineRenderer.SetPosition(1, endPos[array[alertIndex]]);

                alphaKeys[0].alpha = (Mathf.Sin(rushAttackAlertTimer) + 1) / 4;
                alphaKeys[1].alpha = (Mathf.Sin(rushAttackAlertTimer) + 1) / 4;

                gradient.SetKeys(lineRenderer.colorGradient.colorKeys, alphaKeys);
                lineRenderer.colorGradient = gradient;
                rushAttackAlertTimer2 += Time.deltaTime;
                if (rushAttackAlertTimer2 > 1f)
                {
                    rushAttackAlertTimer = 0;
                    rushAttackAlertTimer2 = 0;
                    alphaKeys[0].alpha = 0;
                    alphaKeys[1].alpha = 0;
                    gradient.SetKeys(lineRenderer.colorGradient.colorKeys, alphaKeys);
                    lineRenderer.colorGradient = gradient;
                    alertIndex++;
                }

            }
            else if (alertIndex >= 3)
            {
                AOETimer += Time.deltaTime;

                if (AOETimer > 1f)
                {
                    AOETimer = 0;
                    GameObject AOE = PoolManager.instance.Get(PoolEnum.Enemy, 2, (startPos[array[AOEIndex]] + endPos[array[AOEIndex]]) / 2, Quaternion.identity);
                    AOE.GetComponent<EnemyAOE>().damage = boss.enemyStat.AOEDamage;
                    AOEIndex++;
                    if (AOEIndex >= 3)
                    {
                        Debug.Log("패턴끝");
                        boss.PatternSwitch();
                    }
                }

            }
        }
    }

    private int[] ShuffleArray(int[] array)
    {
        int random1, random2;
        int temp;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array;
    }
}
