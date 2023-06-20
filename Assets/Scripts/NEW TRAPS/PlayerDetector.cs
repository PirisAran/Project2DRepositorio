using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    PlayerController _player;

    public Action OnPlayerDetected;

    [SerializeField]
    bool _canDetect = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canDetect)
        {
            return;
        }
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


    public void SetCanDetect(bool v)
    {
        _canDetect = v;
    }
}
