using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : MoonSingleton<Game>
{
    //ȫ�ַ������еĵ���ģʽ
    [HideInInspector]
    public ObjectPool objectPool;
    [HideInInspector]
    public Sound sound;
    [HideInInspector]
    public StaticData staticData;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        objectPool = ObjectPool.Instance;
        sound = Sound.Instance;
        staticData = StaticData.Instance;

        //��Ϸ����
        //��ʼ��
        RegisterController(Consts.E_StartUp, typeof(StartUpController));
        //��ɳ�����ת
        Game.Instance.LoadLevl(1);
    }
    
    public void LoadLevl(int level)
    {
        //�˳������¼�
        ScenesArgs e = new()
        {
            //��ȡ��ǰ��������ֵ
            ScenesIndex = SceneManager.GetActiveScene().buildIndex
        };
        SendEvent(Consts.E_ExitScenes, e);

        //�����µĳ����¼�
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
    //ע���¼�������ȡ������
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // �����³���ʱ�������¼�
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("�����³�����" + scene.buildIndex);
        // ���볡���¼�
        ScenesArgs e = new()
        {
            // ��ȡ��ǰ��������ֵ
            ScenesIndex = scene.buildIndex
        };
        SendEvent(Consts.E_EnterScenes, e);
    }


    /* private void OnLevelWasLoaded(int level)
    {
        Debug.Log("�����³�����"+level);
        //j���볡���¼�
        ScenesArgs e = new ScenesArgs();
        //��ȡ��ǰ��������ֵ
        e.ScenesIndex = level;
        SendEvent(Consts.E_EnterScenes, e);
    }*/


    void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    //ע��controller
    protected void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }
}