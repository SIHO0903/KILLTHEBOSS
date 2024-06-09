using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossImageSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite BossImage { get; private set; }
    [field: SerializeField] public string SceneName { get; private set; }
}
