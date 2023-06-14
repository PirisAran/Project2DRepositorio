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
    static Vector2 _minVelocity;

    [SerializeField]
    float _maxAddedSpeed = 2;

    [SerializeField]
    float _respectDistance = 10;

    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    void FixedUpdate()
    {
        Vector2 dir = _minVelocity.normalized;
        float minVelMagnitude = _minVelocity.magnitude;

        //float desiredVelMagnitude = GetDesiredVelocityMagnitude(minVelMagnitude);

        //if (desiredVelMagnitude < minVelMagnitude) desiredVelMagnitude = minVelMagnitude;

        _rb.velocity = dir * minVelMagnitude;
        
    }

    private float GetDesiredVelocityMagnitude(float currentMinVel)
    {
        float speed = 0;

        float currentMaxVel = currentMinVel + _maxAddedSpeed;

        float currentDistToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        speed = (currentDistToPlayer > _respectDistance) ? currentMaxVel : currentMinVel;
        
        return speed;
    }

    public static void ModifyVelocity(Vector2 velocityMod)
    {
        _minVelocity += velocityMod;
    }
}
