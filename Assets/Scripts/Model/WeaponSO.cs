using UnityEngine;

namespace Model
{
    [CreateAssetMenu]
    public class WeaponSO : ItemSO
    {
        public enum WeaponType { Melee, Magic };
        [Header("Info")]
        [SerializeField] public WeaponType weaponType;
        public float damage;
        public float attackRate;
        public float criticalChance;
        [HideInInspector] public Color currentDamageTxtColor;
        public Color damageTxtColor;
        public Color criticalDamageTxtColor;
        [HideInInspector] public float currentFontSize;
        public float fontSize;
        public float criticalDamageFontSize;

    }
}