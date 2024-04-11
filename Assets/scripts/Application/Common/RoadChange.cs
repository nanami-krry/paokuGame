using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoadChange : MonoBehaviour
{
    GameObject roadNow;
    GameObject roadNext;
    GameObject parent;
    public string[] roadNames = { "1111", "2222", "3333", "4444" };

    void Start()
    {
        if (parent == null)
        {
            parent = new GameObject();
            parent.transform.position = Vector3.zero;
            parent.name = "Road";
        }
        roadNow = SpawnRandomRoad();
        roadNext = SpawnRandomRoad();
        roadNext.transform.position += new Vector3(0, 0, 144);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tag.road)
        {
            // ���յ�ǰ Road
            Game.Instance.objectPool.Unspawn(other.gameObject);
            // ����һ֡������һ�� Road
            StartCoroutine(SpawnNextRoadNextFrame());
        }
    }

    IEnumerator SpawnNextRoadNextFrame()
    {
        yield return null; // �ȴ���һ֡
        SpawanNewRoad();
    }

    void SpawanNewRoad()
    {
        roadNow = roadNext;
        roadNext = SpawnRandomRoad();
        roadNext.transform.position = roadNow.transform.position + new Vector3(0, 0, 144);
    }

    GameObject SpawnRandomRoad()
    {
        int randomIndex = Random.Range(0, roadNames.Length);
        return Game.Instance.objectPool.Spawn(roadNames[randomIndex], parent.transform);
    }
}

