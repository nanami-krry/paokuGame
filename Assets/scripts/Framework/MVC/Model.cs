using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model : MonoBehaviour
{
    public abstract string Name { get; }
    //�����¼�
    protected void SendEvent(string eventName,object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
}
