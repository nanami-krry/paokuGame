using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MVC
{
    //资源
    public static Dictionary<string, Model> Models = new Dictionary<string, Model>();
    public static Dictionary<string, View> Views = new Dictionary<string, View>();
    public static Dictionary<string, Type> CommandMap = new Dictionary<string, Type>();
    //注册
    public static void RegisterView(View view)
    {
        //防止view重复注册
        if (Views.ContainsKey(view.Name))
        {
            Views.Remove(view.Name);
        }
        view.RegisterAttentionEvent();
        Views[view.Name] = view;
    }
    public static void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }
    public static void RegisterController(string eventName, Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }
    //获取模型model
    public static T GetModel<T>()
    where T : Model
    {
        foreach (var m in Models.Values)
        {
            if (m is T)
            {
                return (T)m;
            }
        }
        return null;
    }
    public static T GetView<T>()
    where T : View
    {
        foreach (var v in Views.Values)
        {
            if (v is T)
            {
                return (T)v;
            }
        }
        return null;
    }
    //发送事件,控制器先接收
    public static void SendEvent(string eventName, object data = null)
    {
        if (CommandMap.ContainsKey(eventName))
        {
            Type t = CommandMap[eventName];
            //控制器生成


            Controller c = Activator.CreateInstance(t) as Controller;
            c.Execute(data);
        }
        //视图处理
        foreach (var v in Views.Values)
        {
            if (v.AttentionList.Contains(eventName))
            {
                v.HandleEvent(eventName, data);
            }
        }
    }
}