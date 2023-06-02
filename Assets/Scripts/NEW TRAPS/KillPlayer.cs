using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class KillPlayer : MonoBehaviour
{
    [SerializeField] bool _canKill;

    PlayerController _player;
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canKill) return;

        if (collision.transform == _player.transform)
        {
            OnKillPlayer();
        }
    }

    private void OnKillPlayer()
    {
        _player.GetComponent<HealthSystem>().KillPlayer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_canKill) return;

        if (collision.gameObject.transform == _player.transform)
        {
            OnKillPlayer();
        }
    }

    public void SetCanKill(bool v)
    {
        _canKill = v;
    }

    // Start is called before the first frame update
}
