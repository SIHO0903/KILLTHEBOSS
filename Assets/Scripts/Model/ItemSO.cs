using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ItemSO : ScriptableObject
{
    public enum TypeEnum { Helmet,Armor,Boots, Accessories, Consumable,Ammo,ingredient,Weapon };
    public Sprite itemImage;

    public int ID => GetInstanceID();
    [field: SerializeField] public bool IsStackable { get; private set; }
    [field: SerializeField] public int Maxquantity { get; private set; } = 1;
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public TypeEnum Type { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int BuyCost { get; private set; }
    [field: SerializeField] public int SellCost { get;  private set; }

    //�������� ȿ��
    [SerializeField] public EffectSO[] effects;

    //������۽� �ʿ��� �����
    [SerializeField] public Ingredient[] ingredient;
}
[System.Serializable]
public class Ingredient
{
    public ItemSO ingredient;
    public int count;
}
