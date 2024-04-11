using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts 
{
    public const string E_ExitScenes = "E_ExitScenes";
    public const string E_EnterScenes = "E_EnterScenes";
    public const string E_StartUp = "E_StartUp";
    public const string E_EndGame = "E_EndGame";
    //model������
    public const string M_GameModel = "M_GameModel";
    //view������
    public const string V_PlayerMove = "V_PlayerMove";
    public const string V_PlayerAnim= "V_PlayerAnim";
}
//ö������
public enum InputDirection{
    NULL,
    Right,
    Left,
    Down,
    Up
}