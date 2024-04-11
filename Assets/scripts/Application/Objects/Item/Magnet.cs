using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : Item
{

    public override void HitPlayer(Transform pos)
    {

        //2.…˘“Ù
        Game.Instance.sound.PlayEffect("Se_UI_Stars");

        //3.ªÿ ’
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
            other.SendMessage("HitMagnet", SendMessageOptions.RequireReceiver);
            //other.SendMessage("HitItem", ItemKind.ItemMagnet);
        }
    }
}
