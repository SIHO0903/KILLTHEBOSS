using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu]
    public class EnemyStatsSO : ScriptableObject
    {
        [Header("Stats")]
        public float maxHealth;
        public float curHealth;
        public float moveSpeed;


        [Space(20)]

        [Header("Damage")]
        public float projectileDamage;
        public float AOEDamage;
        public float contactDamage;

    }
}