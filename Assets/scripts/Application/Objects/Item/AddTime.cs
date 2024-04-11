using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTime : Item
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.player)
        {
            HitPlayer(other.transform);
            other.SendMessage("HitAddTime", SendMessageOptions.RequireReceiver);
        }
    }

    public override void HitPlayer(Transform pos)
    {
        //2.����
        Game.Instance.sound.PlayEffect("Se_UI_Time");

        //3.����
        //Game.Instance.objectPool.Unspawn(gameObject);
        Destroy(gameObject);
    }
}
