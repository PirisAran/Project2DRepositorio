using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour
{
    Transform _player;
    
    SpriteRenderer _sr;
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _sr.enabled = false;
    }
    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player)
        {
            collision.GetComponent<HealthSystem>().KillPlayer();
        }

    }
}
