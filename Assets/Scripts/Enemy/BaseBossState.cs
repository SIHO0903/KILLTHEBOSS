using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossState<T>
{
    public abstract void EnterState(T boss,Transform player);
    public abstract void UpdateState(T boss,Transform player);

}
