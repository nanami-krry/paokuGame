using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : Controller
{
    public override void Execute(object data)
    {
        GameModel gm = GetModel<GameModel>();
        gm.IsPlay = false;
    }
}
