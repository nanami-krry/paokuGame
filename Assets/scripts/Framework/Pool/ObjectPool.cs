using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MoonSingleton<ObjectPool>
{
    //��ԴĿ¼
    public string ResourceDir = "";
    Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();
    //���ӳ���ȡ�����壻
    public GameObject Spawn(string name,Transform trans)
    {
        SubPool pool = null;
        if (!m_pools.ContainsKey(name))
        {
            RegieterNew(name,trans);
        }
        pool = m_pools[name];
        return pool.Spawn();
    }
    //���յ���
    public void Unspawn(GameObject go)
    {
        SubPool pool = null;
        foreach(var p in m_pools.Values)
        {
            if (p.Contain(go))
            {
                pool = p;
                break;
            }
        }
        pool.UnSpawn(go);
    }
    //��������
    public void UnspawnAll()
    {
        foreach(var p in m_pools.Values)
        {
            p.UnspawnAll();
        }
    }
    //�½��ӳ���
    void RegieterNew(string names,Transform trans)
    {
        //��ԴĿ¼
        string path = ResourceDir + "/" + names;
        //����Ԥ����
        GameObject go = Resources.Load<GameObject>(path);
        //�½�һ������
        SubPool pool = new SubPool(trans, go);
        m_pools.Add(pool.Name, pool);
    }
}
