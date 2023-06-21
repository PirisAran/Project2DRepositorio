using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _player;

    [SerializeField]
    static Vector2 _minVelocity;

    [SerializeField]
    float _maxAddedSpeed = 3;

    [SerializeField]
    float _respectDistance = 4;

    [SerializeField] float _maxDistance;

    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    void FixedUpdate()
    {
        Vector2 dir = _minVelocity.normalized;
        float velMagnitude = _minVelocity.magnitude;
        if (!(DifficultyManager._currentDifficultyLevel == DifficultyManager.DifficultyLevels.Normal))
        {
            velMagnitude =+ GetAddedSpeed();
        }

        _rb.velocity = dir * velMagnitude;
        
    }

    private float GetAddedSpeed()
    {
        float distanceBossToPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distanceBossToPlayer < _respectDistance)
        {
            return 0;
        }

        float maxDistanceInterval = _maxDistance - _respectDistance;
        float currentDistanceInterval = distanceBossToPlayer - _respectDistance;

        float addedSpeed = Mathf.Lerp(0, _maxAddedSpeed, Mathf.Clamp01(currentDistanceInterval / maxDistanceInterval));

        return addedSpeed;
    }

    public static void ModifyVelocity(Vector2 velocityMod)
    {
        _minVelocity += velocityMod;
    }
}
