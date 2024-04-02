using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    //物体集合
    List<GameObject> m_objects = new List<GameObject>();
    //预设
    GameObject m_prefab;
    //名字
    public string Name
    {
        get
        {
            return m_prefab.name;
        }
    }
    //父物体的位置
    Transform m_parent;
    public SubPool(Transform parent, GameObject go)
    {
        m_prefab = go;
        m_parent = parent;
    }
    //生成取出
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
    //回收单个物体
    public void UnSpawn(GameObject go)
    {
        if (Contain(go))
        {
            go.SendMessage("OnUnSpawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }
    //回收多个物体
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
    //判断是否在数据集合里面
    public bool Contain(GameObject go)
    {
        return m_objects.Contains(go);
    }
}
