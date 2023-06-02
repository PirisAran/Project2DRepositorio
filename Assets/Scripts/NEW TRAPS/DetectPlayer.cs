using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    PlayerController _player;

    public Action OnPlayerDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
        {
            OnPlayerDetected?.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
    }

}
