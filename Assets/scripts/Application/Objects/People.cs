using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : Obstacles
{

    public bool isHit = false;
    public float speed = 10;
    public bool isFly = false;
   // GameModel gm;
    Animation anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animation>();

       // gm = MVC.GetModel<GameModel>();
       // StartCoroutine(huishou());
    }

    public override void HitPlayer(Vector3 pos)
    {
        //1.��Ч
        GameObject go = Game.Instance.objectPool.Spawn("FX_ZhuangJi", effectParent);
        go.transform.position = pos;
        isHit = false;
        isFly = true;
        anim.Play("fly");
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        anim.Play("run");
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        anim.transform.localPosition = Vector3.zero;
        isHit = false;
        isFly = false;

    }

    //��ʼ�ƶ�
    public void HitTrigger()
    {
        isHit = true;
    }

    private void Update()
    {
        if (isHit)
        {
            transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        if (isFly)
        {
            transform.position += new Vector3(0,speed, speed) * Time.deltaTime;
        }
    }

    /*IEnumerator huishou()
    {

        yield return new WaitForSeconds(3);
        Game.Instance.objectPool.Unspawn(gameObject);
    }*/
}
