using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class HealthSO : EffectSO
{
    public override void Apply(PlayerController playerController)
    {
        playerController.Health(CureHealthAmount);
    }

}
