using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowPlayer : MonoBehaviour
{
    Transform m_player;
    Vector3 m_offest;
    float speed = 20;
    private void Awake()
    {
        m_player = GameObject.FindWithTag(Tag.player).transform;
        m_offest = transform.position - m_player.position;
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, m_offest + m_player.position, speed * Time.deltaTime);
    }
}
