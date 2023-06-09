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
        _minVelocity = Vector2.zero;
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        if (!(DifficultyManager._currentDifficultyLevel == DifficultyManager.DifficultyLevels.Hard))
        {
            _maxAddedSpeed = 1.5f;
        }
    }

    void FixedUpdate()
    {
        Vector2 dir = _minVelocity.normalized;
        float velMagnitude = _minVelocity.magnitude + GetAddedSpeed();

        _rb.velocity = dir * velMagnitude;
        Debug.Log(_rb.velocity.x + " || " + _rb.velocity.y);

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
