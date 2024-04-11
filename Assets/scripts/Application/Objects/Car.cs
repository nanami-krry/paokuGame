using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Obstacles
{
    public bool carMove = false;
    bool isBlock = false;
    public float speed = 10;
    protected override void Awake()
    {
        
    }
    public override void HitPlayer(Vector3 pos)
    {
        base.HitPlayer(pos);
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
    }
    public override void OnUnSpawn()
    {
        isBlock = false;
        base.OnUnSpawn();
    }
    //角色碰撞到触发区域
    public void HitTrigger()
    {
        isBlock = true;
    }
    private void Update()
    {
        if (isBlock && carMove)
        {
            transform.Translate(-transform.forward * speed * Time.deltaTime);
        }
    }
}
