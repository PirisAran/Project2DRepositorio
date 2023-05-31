using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class Geyser : MonoBehaviour, IDamageFire
{
    private enum States
    {
        inactive,
        charging,
        active
    }
    public float DamageDealt => _damage;
    float _damage = 999; 

    States _currentState;

    [SerializeField] Transform _player;

    [SerializeField]
    float _inactiveTime, _chargingTime, _activeTime;
    float _currentTime,_initTime;
    [SerializeField]
    float _chargingSpeed, _activeSpeed;

    ParticleSystem _particleSystem;
    ParticleSystem.EmissionModule _particleEmission;
    ParticleSystem.MainModule _mainModule;

    Collider2D _trigerGeyser;


    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleEmission = _particleSystem.emission;
        _mainModule = _particleSystem.main;

        _trigerGeyser = GetComponentInParent<Collider2D>();
    }
    private void Start()
    {
        var gameLogic = GameLogic.GetGameLogic();
        var gameController = gameLogic.GetGameController();

        _player = gameController.m_Player.transform;
        _currentState = States.inactive;
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.inactive:
                UpdateInactive();
                break;
            case States.charging:
                UpdateCharging();
                break;
            case States.active:
                UpdateActive();
                break;
            default:
                break;
        }
    }

    private void UpdateInactive()
    {
        _currentTime = Time.time - _initTime;

        _particleEmission.enabled = false;
        _trigerGeyser.enabled = false;

        if (_currentTime >= _inactiveTime)
        {
            _initTime = Time.time;
            ChangeState(States.charging);
        }
    }
    private void UpdateCharging()
    {
        _currentTime = Time.time - _initTime;

        _particleEmission.enabled = true;
        _mainModule.startSpeed = _chargingSpeed;

        if (_currentTime >= _chargingTime)
        {
            _initTime = Time.time;
            ChangeState(States.active);
        }
    }
    private void UpdateActive()
    {
        _currentTime = Time.time - _initTime;

        _mainModule.startSpeed = _activeSpeed;
        _trigerGeyser.enabled = true;
      
        if (_currentTime >= _activeTime)
        {
            _initTime = Time.time;
            ChangeState(States.inactive);
        }
    }

    private void ChangeState(States nextState)
    {
        _currentState = nextState;
    }
}
