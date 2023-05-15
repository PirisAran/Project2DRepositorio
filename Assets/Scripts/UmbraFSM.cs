using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbraFSM : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    FireController _fire;

    //Player Components
    Runner _runner;
    Thrower _thrower;

    //FSM variables
    [SerializeField]
    States _currentState;
    States _nextState;
    [SerializeField]
    float _cuteSpeed, _followSpeed, _killerSpeed;
    [SerializeField]
    float _currentSpeed;
    [SerializeField]
    float _cuteAccelerationTime, _followAccelerationTime, _killerAccelerationTime;
    [SerializeField]
    float _transitionTime;
    float _timeCurrentState;

    public Action OnEnterCuteState;
    public Action OnEnterFollowState;
    public Action OnEnterKillerState;
    public Action OnEnterTransitionState;

    private Vector2 _playerDirection => (_player.transform.position - transform.position).normalized;
    private Vector2 _fireDirection => (_fire.transform.position - transform.position).normalized;

    private float _lightRange => _fire.LightRange;

    //FSM States
    private enum States { Cute, Follow, Killer, Transition}

    private void Awake()
    {
        GetPlayerComp();
        Init();
    }
    private void Init()
    {
        _currentState = States.Follow;
        _timeCurrentState = 0;
    }
    private void GetPlayerComp()
    {
        _runner = _player.GetComponent<Runner>();
        _thrower = _player.GetComponent<Thrower>();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case States.Cute:
                UpdateCuteState();
                break;
            case States.Follow:
                UpdateFollowState();
                break;
            case States.Killer:
                UpdateKillerState();
                break;
            case States.Transition:
                UpdateTransitionState();
                break;
            default:
                break;
        }
        _timeCurrentState += Time.deltaTime;
    }

    private void OnEnterState(States state)
    {
        switch (state)
        {
            case States.Cute:
                OnEnterCuteState?.Invoke();
                break;
            case States.Follow:
                OnEnterFollowState?.Invoke();
                break;
            case States.Killer:
                OnEnterKillerState?.Invoke();
                break;
            case States.Transition:
                OnEnterTransitionState?.Invoke();
                break;
            default:
                break;
        }
    }

    private void TransitionToState(States nextState)
    {
        _currentState = States.Transition;
        _nextState = nextState;
        OnEnterState(States.Transition);
        _timeCurrentState = 0;
    }

    private void UpdateCuteState()
    {
        //Antes de nada, comprovar si puede cambiar de estados
        if (CanEnterFollowState())
        {
            TransitionToState(States.Follow);
            return;
        }
        if (CanEnterKillerState())
        {
            TransitionToState(States.Killer);
            return;
        }

        float desiredSpeed = _cuteSpeed;
        float timeToAccelerate = _cuteAccelerationTime;
        _currentSpeed = GetCurrentSpeed(desiredSpeed, timeToAccelerate);
        RunFromFire(_currentSpeed);
    }

    private float GetCurrentSpeed(float desiredSpeed, float timeToAccelerate)
    {
        float t = Mathf.Clamp01(_timeCurrentState / timeToAccelerate);
        return Mathf.Lerp(_currentSpeed, desiredSpeed, t);
    }

    private void UpdateFollowState()
    {
        //Antes de nada, comprovar si puede cambiar de estados
        if (CanEnterCuteState())
        {
            TransitionToState(States.Cute);
            return;
        }
        if (CanEnterKillerState())
        {
            TransitionToState(States.Killer);
            return;
        }

        //FER AMB DESIRED POSITION
        float acceleration = 2;
        
        //Mathf.MoveTowardsTo
        //Sha de moure sobre una mateixa recta, si es pasa del punt, ha de frenar y despres accelerar. Si encara no arriba, ha d accelerar y frenar dps.
       
    }

    private void UpdateKillerState()
    {
        //Antes de nada, comprovar si puede cambiar de estados
        if (CanEnterCuteState())
        {
            TransitionToState(States.Cute);
            return;
        }
        if (CanEnterFollowState())
        {
            TransitionToState(States.Follow);
            return;
        }

        float desiredSpeed = _killerSpeed;
        float timeToAccelerate = _killerAccelerationTime;
        _currentSpeed = GetCurrentSpeed(desiredSpeed, timeToAccelerate);
        
        MoveTowardsPlayer(_currentSpeed);
    }

    private void UpdateTransitionState()
    {
        if (_timeCurrentState >= _transitionTime)
        {
            _currentState = _nextState;
            OnEnterState(_currentState);
            return;
        }
        float desiredSpeed = 0;
        float timeToAccelerate = _transitionTime;
        _currentSpeed = GetCurrentSpeed(desiredSpeed, timeToAccelerate);
        Debug.Log("Transition time "+ _timeCurrentState);
    }

    private void MoveTowardsPlayer(float speed)
    {
        transform.Translate(_playerDirection * speed * Time.deltaTime);
    }
    private void RunFromFire(float speed)
    {
        transform.Translate(-_fireDirection * speed * Time.deltaTime);
    }
    private bool IsInLightRange()
    {
        return Vector2.Distance(transform.position, _fire.transform.position) < _lightRange;
    }
    private bool IsPlayerSafe()
    {
        return Vector2.Distance(_player.transform.position, _fire.transform.position) <= _lightRange;
    }

    private bool CanEnterCuteState()
    {
        return IsInLightRange();
    }
    private bool CanEnterFollowState()
    {
        return !IsInLightRange() && IsPlayerSafe();
    }
    private bool CanEnterKillerState()
    {
        return !IsInLightRange() && !IsPlayerSafe();
    }
}
