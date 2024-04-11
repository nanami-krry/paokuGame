using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterScenceControlller : Controller
{
    public override void Execute(object data)
    {
        ScenesArgs e = data as ScenesArgs;
        switch (e.ScenesIndex)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                RegisterView(GameObject.FindWithTag(Tag.player).GetComponent<PlayerMove>());
                RegisterView(GameObject.FindWithTag(Tag.player).GetComponent<PlayerAnim>());
                break;
        }
    }
}
