using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{

    Transform effectParent;
   public float moveSpeed = 40;

    public override void HitPlayer(Transform pos)
    {
        //1.��Ч
        GameObject go = Game.Instance.objectPool.Spawn("FX_JinBi", effectParent);
        go.transform.position = pos.position;

        //2.����
        Game.Instance.sound.PlayEffect("Se_UI_JinBi");

        //3.����
        //Game.Instance.objectPool.Unspawn(gameObject);
        Destroy(gameObject);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.player)
        {
            HitPlayer(other.transform);
            other.SendMessage("HitCoin", SendMessageOptions.RequireReceiver);
        }
        else if (other.tag == Tag.magnetColider)
        {
            //��������
            StartCoroutine(HitMagnet(other.transform));
        }
    }

    IEnumerator HitMagnet(Transform pos)
    {
        bool isLoop = true;
        while (isLoop)
        {
            transform.position = Vector3.Lerp(transform.position, pos.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, pos.position) < 0.5f)
            {
                isLoop = false;
                HitPlayer(pos.transform);
                pos.parent.SendMessage("HitCoin", SendMessageOptions.RequireReceiver);
            }
            yield return 0;
        }
    }


    private void Awake()
    {
        effectParent = GameObject.Find("EffectParent").transform;
    }
}
