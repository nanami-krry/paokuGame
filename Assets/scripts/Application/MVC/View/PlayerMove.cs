using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : View
{
    //常量
    const float grivaty = 9.8f;
    const float m_jumpValue = 5;
    const float m_moveSpeed = 13;
    //速度
    const float m_SpeedAddDis = 200;
    const float m_SpeedAddRate =0.5f;
    const float m_MaxSpeed = 40;
    //字段
    public float speed =20;
    CharacterController m_cc;
    InputDirection m_inputDir = InputDirection.NULL;
    Vector3 m_mousePos;
    int m_nowIndex = 1;
    int m_targetIndex = 1;
    float m_xDistance;
    float m_yDistance;

    //滑动
    bool m_IsSlide = false;
    float m_SlideTime;
    float m_SpeedAddCount;

    GameModel gm;
    //记录速度
    float m_Maskspeed;
    //增加速度的速率
    float m_AddRate=10;
    bool m_IsHit = false;
    //Item有关
    public int m_DoublieTime = 1;
    int m_SkillTime;

    IEnumerator MultiplyCor;
    IEnumerator MagnetCor;
    IEnumerator InvincibleCor;
    SphereCollider m_MagnetColider;
    //判断角色是否是无敌状态
    bool m_IsInvincibe = false;
    public override string Name => Consts.V_PlayerMove;
    //角色最大速度
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            if (speed > m_MaxSpeed)
            {
                speed = m_MaxSpeed;
            }
        }
    }

    public override void HandleEvent(string name, object data)
    {
        throw new System.NotImplementedException();
    }

    #region 角色移动及控制
    IEnumerator UpdateAction()
    {
        while (true)
        {
            if (!gm.IsPause && gm.IsPlay)
            {
                m_yDistance -= grivaty * Time.deltaTime;
                m_cc.Move((transform.forward * Speed + new Vector3(0, m_yDistance, 0)) * Time.deltaTime);
                MoveControl();
                UpdatePosition();
                UpdateSpeed();
            }
            yield return 0;
        }
    }
    //获取输入
    void GetInputDirection()
    {
        //利用鼠标识别
        m_inputDir = InputDirection.NULL;
        if (Input.GetMouseButtonDown(0))
        {
            m_mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 dir = Input.mousePosition - m_mousePos;
            float xDir = dir.x;
            float yDir = dir.y;
            dir.Normalize();

            if (Mathf.Abs(xDir) > Mathf.Abs(yDir))
            {
                if (xDir > 0)
                    m_inputDir = InputDirection.Right;
                else
                    m_inputDir = InputDirection.Left;
            }
            else
            {
                if (yDir > 0)
                    m_inputDir = InputDirection.Up;
                else
                    m_inputDir = InputDirection.Down;
            }
        }
        //利用按键识别
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            m_inputDir = InputDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            m_inputDir = InputDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            m_inputDir = InputDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            m_inputDir = InputDirection.Right;
        }
        //输出
        print(m_inputDir);
    }
    //更新角色所在跑道位置
    void UpdatePosition()
    {
        GetInputDirection();
        switch (m_inputDir)
        {
            case InputDirection.NULL:
                break;
            case InputDirection.Right:
                if (m_targetIndex < 2)
                {
                    m_targetIndex++;
                    m_xDistance = 2;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Huadong");
                }
                break;
            case InputDirection.Left:
                if (m_targetIndex > 0)
                {
                    m_targetIndex--;
                    m_xDistance = -2;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Huadong");
                }
                break;
            case InputDirection.Down:
                if (m_IsSlide == false)
                {

                    m_IsSlide = true;
                    m_SlideTime = 0.733f;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Slide");
                }
                break;
            case InputDirection.Up:
                if (m_cc.isGrounded)
                {
                    m_yDistance = m_jumpValue;
                    SendMessage("AnimManager", m_inputDir);
                    Game.Instance.sound.PlayEffect("Se_UI_Jump");
                }
                break;
            default:
                break;
        }
    }
    //角色移动
    void MoveControl()
    {
        //左右移动
        if (m_targetIndex != m_nowIndex)
        {
            float move = Mathf.Lerp(0, m_xDistance, m_moveSpeed * Time.deltaTime);
            transform.position += new Vector3(move, 0, 0);
            m_xDistance -= move;
            if (Mathf.Abs(m_xDistance) < 0.05f)
            {
                m_xDistance = 0;
                m_nowIndex = m_targetIndex;
                switch (m_nowIndex)
                {
                    case 0:
                        transform.position = new Vector3(-2, transform.position.y, transform.position.z);
                        break;
                    case 1:
                        transform.position = new Vector3(0, transform.position.y, transform.position.z);
                        break;
                    case 2:
                        transform.position = new Vector3(2, transform.position.y, transform.position.z);
                        break;
                }
            }
        }
        if (m_IsSlide)
        {
            m_SlideTime -= Time.deltaTime;
            if (m_SlideTime < 0)
            {
                m_IsSlide = false;
                m_SlideTime = 0;
            }
        }
    }
    //更新速度,当角色跑了一定米数后增加角色速度；
    void UpdateSpeed()
    {
        m_SpeedAddCount += Speed * Time.deltaTime;
        if (m_SpeedAddCount > m_SpeedAddDis)
        {
            m_SpeedAddCount = 0;
            Speed += m_SpeedAddRate;
        }
    }
    #endregion

    #region 角色响应时间
    public void HitObstacles()
    {
        if (m_IsHit)
            return;
        m_IsHit = true;
        m_Maskspeed = Speed;
        Speed = 0;
        StartCoroutine(DecreaseSpeed());
    }
    IEnumerator DecreaseSpeed()
    {
        while (Speed <=m_Maskspeed)
        {
            Speed += Time.deltaTime * m_AddRate;
            yield return 0;
        }
        m_IsHit = true;
    }

    //吃金币
    public void HitCoin()
    {
        print("吃金币");
    }
    //双倍金币
    public void HitMultiply()
    {
        if (MultiplyCor != null)
        {
            StopCoroutine(MultiplyCor);

        }
        MultiplyCor = MultiplyCoroutine();
        StartCoroutine(MultiplyCor);
    }
    IEnumerator MultiplyCoroutine()
    {
        m_DoublieTime = 2;
        yield return new WaitForSeconds(m_SkillTime);
        m_DoublieTime = 1;
    }

    //吸铁石
    public void HitMagnet()
    {
        if (MagnetCor != null)
        {
            StopCoroutine(MagnetCor);

        }
        MagnetCor = MagnetCoroutine();
        StartCoroutine(MagnetCor);
    }
    IEnumerator MagnetCoroutine()
    {
        m_MagnetColider.enabled = true;
        yield return new WaitForSeconds(m_SkillTime);
        m_MagnetColider.enabled = false;

    }
    //加时间
    public void HitAddTime()
    {
        print("时间增加");
    }

    //角色无敌
    public void HitInvincible()
    {
        if (InvincibleCor != null)
        {
            StopCoroutine(InvincibleCor);

        }
        InvincibleCor = InvincibleCoroutine();
        StartCoroutine(InvincibleCor);
    }
    IEnumerator InvincibleCoroutine()
    {
        m_IsInvincibe= true;
        yield return new WaitForSeconds(m_SkillTime);
        m_IsInvincibe= false;

    }

    #endregion

    #region unity回调
    //触发响应事件
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tag.smallFence)
        {
            if (m_IsInvincibe)
                return;
            other.gameObject.SendMessage("HitPlayer",transform.position);
            HitObstacles();
            //撞击时声音
            Game.Instance.sound.PlayEffect("Se_UI_Hit");
        }
        else if (other.gameObject.tag == Tag.bigFence)
        {
            if (m_IsInvincibe)
                return;
            if (m_IsSlide)
                return;
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //撞击时声音
            Game.Instance.sound.PlayEffect("Se_UI_Hit");
            HitObstacles();
        }
        else if (other.gameObject.tag == Tag.block)//游戏结束
        {
            Game.Instance.sound.PlayEffect("Se_UI_End");
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //游戏结束
            SendEvent(Consts.E_EndGame);
        }
        else if (other.gameObject.tag == Tag.beforeTrigger)//汽车
        {
            
            other.transform.parent.SendMessage("HitTrigger", SendMessageOptions.RequireReceiver);
            
        }
    }
    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        gm = GetModel<GameModel>();
        m_SkillTime = gm.SkillTime;
        //获取MagnetColider
        m_MagnetColider = GetComponentInChildren<SphereCollider>();
        m_MagnetColider.enabled = false;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
            //gm.IsPause = true;
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    gm.IsPause = false;
        //}
    }
    private void Start()
    {
        StartCoroutine(UpdateAction());
    }
    #endregion
}