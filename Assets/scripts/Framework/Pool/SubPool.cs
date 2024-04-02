using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    //���弯��
    List<GameObject> m_objects = new List<GameObject>();
    //Ԥ��
    GameObject m_prefab;
    //����
    public string Name
    {
        get
        {
            return m_prefab.name;
        }
    }
    //�������λ��
    Transform m_parent;
    public SubPool(Transform parent, GameObject go)
    {
        m_prefab = go;
        m_parent = parent;
    }
    //����ȡ��
    public GameObject Spawn()
    {
        GameObject go = null;
        foreach (var obj in m_objects)
        {
            if (!obj.activeSelf)
            {
                go = obj;
            }
        }
        if (go == null)
        {
            go = GameObject.Instantiate<GameObject>(m_prefab);
            go.transform.parent = m_parent;
            m_objects.Add(go);
        }
        go.SetActive(true);
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }
    //���յ�������
    public void UnSpawn(GameObject go)
    {
        if (Contain(go))
        {
            go.SendMessage("OnUnSpawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }
    //���ն������
    public void UnspawnAll()
    {
        foreach (var obj in m_objects)
        {
            if (obj.activeSelf)
            {
                UnSpawn(obj);
            }
        }
    }
    //�ж��Ƿ������ݼ�������
    public bool Contain(GameObject go)
    {
        return m_objects.Contains(go);
    }
}
