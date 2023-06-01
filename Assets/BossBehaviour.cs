using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _player;
    [SerializeField] float _xSpeedMax = 10, _ySpeedMax = 200;

    [SerializeField]
    static Vector2 _currentVelocity;

    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    void FixedUpdate()
    {
        _rb.velocity = _currentVelocity;
    }

    public static void ModifyVelocity(Vector2 velocityMod)
    {
        _currentVelocity += velocityMod;
    }
}
