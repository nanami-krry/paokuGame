using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : ReusableObject
{
    protected Transform effectParent;
    protected virtual void Awake()
    {
        effectParent = GameObject.Find("EffectParent").transform;
    }
    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        
    }
    public virtual void HitPlayer(Vector3 pos)
    {
        //ײ����Ч
        GameObject go= Game.Instance.objectPool.Spawn("FX_ZhuangJi",effectParent);
        go.transform.position = pos;
        
        //����
        //Game.Instance.objectPool.Unspawn(gameObject);
        Destroy(gameObject);
    }
}
