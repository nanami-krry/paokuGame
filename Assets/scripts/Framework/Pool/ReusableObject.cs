using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReusableObject : MonoBehaviour, IReusable
{
    //抽象方法,不可以实例化
    public abstract void OnSpawn();

    public abstract void OnUnSpawn();
   
}
