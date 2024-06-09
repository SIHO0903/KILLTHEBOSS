using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public abstract class EffectSO : ScriptableObject
{
    [field: SerializeField] public float DefenseAmount { get; private set; }
    [field: SerializeField] public float AddHealthAmount { get; private set; }
    [field: SerializeField] public float CureHealthAmount { get; private set; }
    [field: SerializeField] public float DamageAmount { get; private set; }
    [field: SerializeField] public bool canDash { get; private set; }

    public abstract void Apply(PlayerController playerController);
}
