using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class StatsSO : EffectSO
{
    public override void Apply(PlayerController playerController)
    {
        playerController.AddEffect(this);
    }
}
