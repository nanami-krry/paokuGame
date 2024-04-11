using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Execute(object data)
    {
        //注册所有的controller
        RegisterController(Consts.E_EnterScenes, typeof(EnterScenceControlller));
        RegisterController(Consts.E_EndGame, typeof(EndGameController));
        //注册Model
        RegisterModel(new GameModel());
        //初始化
        
    }
}
