using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Execute(object data)
    {
        //ע�����е�controller
        RegisterController(Consts.E_EnterScenes, typeof(EnterScenceControlller));
        RegisterController(Consts.E_EndGame, typeof(EndGameController));
        //ע��Model
        RegisterModel(new GameModel());
        //��ʼ��
        
    }
}
