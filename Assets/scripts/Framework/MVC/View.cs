using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    //���ֱ�ʶ
    public abstract string Name { get; }
    [HideInInspector]
    public List<string> AttentionList = new List<string>();

    public virtual void RegisterAttentionEvent()
    {

    }
    //�����¼�
    public abstract void HandleEvent(string name, object data);
    //�����¼�
    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
    //��ȡģ��
    protected T GetModel<T>()
        where T:Model
    {
        return MVC.GetModel<T>() as T;
    }
}
