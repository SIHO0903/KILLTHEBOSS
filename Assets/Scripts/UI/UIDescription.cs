using Model;
using System.Text;
using TMPro;
using UnityEngine;

public class UIDescription : MonoBehaviour
{
    [SerializeField] TMP_Text titleTxt;

    private void Awake()
    {
        Toggle(false);
    }
    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
    public void SetData(StringBuilder stringBuilder)
    {
        titleTxt.text = stringBuilder.ToString();
        gameObject.SetActive(true);
    }

    //아이템 설명 메소드
    public StringBuilder Desc(ItemSO item)
    {
        StringBuilder desc = new StringBuilder();
        desc.AppendLine(item.Name);
        desc.AppendLine(item.Type.ToString());
        desc.AppendLine(item.Description);
        EffectSO[] effects = item.effects;
        WeaponSO weaponItem = item as WeaponSO;
        ProjectileSO projectile = item as ProjectileSO;
        if (weaponItem != null)
        {
            AppendLineIfNotZero(desc, "Weapon Type : ", weaponItem.weaponType.ToString());
            AppendLineIfNotZero(desc, "Damage : ", weaponItem.damage);
            AppendLineIfNotZero(desc, "AttackSpeed : ", weaponItem.attackRate);
            AppendLineIfNotZero(desc, "Critical Chance : ", weaponItem.criticalChance + "%");
        }
        if(projectile != null)
        {
            AppendLineIfNotZero(desc, "Ammo Damage : ", projectile.damage);
        }
        if(effects.Length > 0)
        {
            AppendLineIfNotZero(desc, "Recovery Health : ", effects[0].CureHealthAmount);
            AppendLineIfNotZero(desc, "Add Health : ", effects[0].AddHealthAmount);
            AppendLineIfNotZero(desc, "Add Damage : ", effects[0].DamageAmount);
            AppendLineIfNotZero(desc, "Add Defense : ", effects[0].DefenseAmount);
        }

        return desc;
    }

    void AppendLineIfNotZero(StringBuilder sb, string restString, float? value)
    {
        if (value != 0)
        {
            sb.AppendLine(restString +value.ToString());
        }
    }
    void AppendLineIfNotZero(StringBuilder sb, string restString, string value)
    {
        if (value != null)
        {
            sb.AppendLine(restString + value);
        }
    }
}
