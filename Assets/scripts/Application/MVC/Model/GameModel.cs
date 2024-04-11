using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : Model
{
    
    bool m_IsPlay = true;
    bool m_IsPause = false;
    //双倍金币技能时间
    int m_SkillTime = 5;
    
    public override string Name
    {
        get
        {
            return Consts.M_GameModel;
        }
    }

    public bool IsPlay { get => m_IsPlay; set => m_IsPlay = value; }
    public bool IsPause { get => m_IsPause; set => m_IsPause = value; }
    public int SkillTime { get => m_SkillTime; set => m_SkillTime = value; }
}
