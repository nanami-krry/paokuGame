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
    //全局访问所有的单例模式
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

        //游戏启动
        //初始化
        RegisterController(Consts.E_StartUp, typeof(StartUpController));
        //完成场景跳转
        Game.Instance.LoadLevl(1);
    }
    
    public void LoadLevl(int level)
    {
        //退出场景事件
        ScenesArgs e = new()
        {
            //获取当前场景索引值
            ScenesIndex = SceneManager.GetActiveScene().buildIndex
        };
        SendEvent(Consts.E_ExitScenes, e);

        //加载新的场景事件
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
    //注册事件监听和取消监听
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // 进入新场景时触发的事件
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("进入新场景：" + scene.buildIndex);
        // 进入场景事件
        ScenesArgs e = new()
        {
            // 获取当前场景索引值
            ScenesIndex = scene.buildIndex
        };
        SendEvent(Consts.E_EnterScenes, e);
    }


    /* private void OnLevelWasLoaded(int level)
    {
        Debug.Log("进入新场景："+level);
        //j进入场景事件
        ScenesArgs e = new ScenesArgs();
        //获取当前场景索引值
        e.ScenesIndex = level;
        SendEvent(Consts.E_EnterScenes, e);
    }*/


    void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    //注册controller
    protected void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }
}