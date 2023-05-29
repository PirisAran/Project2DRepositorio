using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _player;
    [SerializeField] float _xSpeedMax = 10, _ySpeedMax = 200;



    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    void Update()
    {
        Vector2 playerDir = (_player.position - transform.position).normalized;

        _rb.velocity = new Vector2(playerDir.x * _xSpeedMax, playerDir.y * _ySpeedMax);

        Debug.Log(_rb.velocity);
    }
}
