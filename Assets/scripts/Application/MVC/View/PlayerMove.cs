using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : View
{
    //����
    const float grivaty = 9.8f;
    const float m_jumpValue = 5;
    const float m_moveSpeed = 13;
    //�ٶ�
    const float m_SpeedAddDis = 200;
    const float m_SpeedAddRate =0.5f;
    const float m_MaxSpeed = 40;
    //�ֶ�
    public float speed =20;
    CharacterController m_cc;
    InputDirection m_inputDir = InputDirection.NULL;
    Vector3 m_mousePos;
    int m_nowIndex = 1;
    int m_targetIndex = 1;
    float m_xDistance;
    float m_yDistance;

    //����
    bool m_IsSlide = false;
    float m_SlideTime;
    float m_SpeedAddCount;

    GameModel gm;
    //��¼�ٶ�
    float m_Maskspeed;
    //�����ٶȵ�����
    float m_AddRate=10;
    bool m_IsHit = false;
    //Item�й�
    public int m_DoublieTime = 1;
    int m_SkillTime;

    IEnumerator MultiplyCor;
    IEnumerator MagnetCor;
    IEnumerator InvincibleCor;
    SphereCollider m_MagnetColider;
    //�жϽ�ɫ�Ƿ����޵�״̬
    bool m_IsInvincibe = false;
    public override string Name => Consts.V_PlayerMove;
    //��ɫ����ٶ�
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

    #region ��ɫ�ƶ�������
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
    //��ȡ����
    void GetInputDirection()
    {
        //�������ʶ��
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
        //���ð���ʶ��
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
        //���
        print(m_inputDir);
    }
    //���½�ɫ�����ܵ�λ��
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
    //��ɫ�ƶ�
    void MoveControl()
    {
        //�����ƶ�
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
    //�����ٶ�,����ɫ����һ�����������ӽ�ɫ�ٶȣ�
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

    #region ��ɫ��Ӧʱ��
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

    //�Խ��
    public void HitCoin()
    {
        print("�Խ��");
    }
    //˫�����
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

    //����ʯ
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
    //��ʱ��
    public void HitAddTime()
    {
        print("ʱ������");
    }

    //��ɫ�޵�
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

    #region unity�ص�
    //������Ӧ�¼�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tag.smallFence)
        {
            if (m_IsInvincibe)
                return;
            other.gameObject.SendMessage("HitPlayer",transform.position);
            HitObstacles();
            //ײ��ʱ����
            Game.Instance.sound.PlayEffect("Se_UI_Hit");
        }
        else if (other.gameObject.tag == Tag.bigFence)
        {
            if (m_IsInvincibe)
                return;
            if (m_IsSlide)
                return;
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //ײ��ʱ����
            Game.Instance.sound.PlayEffect("Se_UI_Hit");
            HitObstacles();
        }
        else if (other.gameObject.tag == Tag.block)//��Ϸ����
        {
            Game.Instance.sound.PlayEffect("Se_UI_End");
            other.gameObject.SendMessage("HitPlayer", transform.position);
            //��Ϸ����
            SendEvent(Consts.E_EndGame);
        }
        else if (other.gameObject.tag == Tag.beforeTrigger)//����
        {
            
            other.transform.parent.SendMessage("HitTrigger", SendMessageOptions.RequireReceiver);
            
        }
    }
    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        gm = GetModel<GameModel>();
        m_SkillTime = gm.SkillTime;
        //��ȡMagnetColider
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