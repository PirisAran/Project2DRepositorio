using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class UmbraFSM : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    FireController _fire;

    [Header("FSM Variables")]
    [SerializeField]States _currentState;
    States _nextState;
    [SerializeField]float _followStateRange = 3f;
    [SerializeField]float _transitionTime = 1f;
    float _timeCurrentState;
    float _speedBeforeTransition;
    [Space]

    [Header("States Base Speed")]
    [SerializeField] float _cuteBaseSpeed = 0f;
    [SerializeField] float _followBaseSpeed = 1f;
    [SerializeField] float _killerBaseSpeed = 6f;
    [Space]

    [Header("States Maximum Speed")]
    [SerializeField]float _cuteMaxSpeed = 2f;
    [SerializeField]float _followMaxSpeed = 10f;
    [SerializeField]float _killerMaxSpeed = 10f;
    [Space]

    [Header("Movement Values")]
    [SerializeField]float _currentSpeed;
    [SerializeField]float _acceleration;

    Vector3 _desiredPosition;

    public Action OnEnterCuteState;
    public Action OnEnterFollowState;
    public Action OnEnterKillerState;
    public Action OnEnterTransitionState;

    public Vector3 Forward => (_desiredPosition - transform.position).normalized;
    public States CurrentState => _currentState;
    private Vector3 _playerDirection => (_player.transform.position - transform.position).normalized;
    private Vector3 _fireDirection => (_fire.transform.position - transform.position).normalized;
    private float _lightRange => _fire.LightRange;
    private float _respectRange => _fire.LightRange + _followStateRange;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_desiredPosition, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_player.transform.position, _respectRange);
        
    }

    //FSM States
    public enum States { Cute, Follow, Killer, Transition}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_currentState == States.Cute || _currentState == States.Transition) return;

        HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
        if (healthSystem != null)
            healthSystem.KillPlayer();
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.gameObject;
        _fire = _player.GetComponentInChildren<FireController>();
    }
    private void Init()
    {
        _currentState = States.Follow;
        _timeCurrentState = 0;
    }

    private void FixedUpdate()
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
        _speedBeforeTransition = _currentSpeed;
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
        
        _desiredPosition = _fire.transform.position + (-_fireDirection) * _respectRange;
        MoveTowardsPosition(_desiredPosition, _cuteBaseSpeed, _cuteMaxSpeed, _acceleration);
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

        _desiredPosition = _player.transform.position + (-_playerDirection) * _respectRange;
        MoveTowardsPosition(_desiredPosition, _followBaseSpeed, _followMaxSpeed, _acceleration);
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

        _desiredPosition = _player.transform.position;
        MoveTowardsPosition(_desiredPosition, _killerBaseSpeed, _killerMaxSpeed, _acceleration);
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
        float timeToDecelerate = _transitionTime;
        _currentSpeed = Mathf.Lerp(_speedBeforeTransition, desiredSpeed, Mathf.Clamp01(_timeCurrentState / timeToDecelerate));
        transform.Translate(Forward * _currentSpeed * Time.fixedDeltaTime);
    }

    private void MoveTowardsPosition(Vector3 desiredPosition, float baseSpeed, float maxSpeed, float acceleration)
    {
        float distance = Vector3.Distance(transform.position, desiredPosition);

        // calcular la velocidad actual del objeto
        _currentSpeed = baseSpeed + acceleration * distance;

        // limitar la velocidad máxima del objeto
        _currentSpeed = Mathf.Clamp(_currentSpeed, baseSpeed, maxSpeed);

        // mover el objeto hacia la posición del mouse con velocidad acelerada
        //transform.position = Vector3.MoveTowards(transform.position, desiredPosition, _currentSpeed * Time.deltaTime);
        float mov = _currentSpeed * Time.fixedDeltaTime;

        if (mov > distance) mov = distance;

        transform.Translate(Forward * mov);
    }
    private bool IsInLightRange()
    {
        return Vector3.Distance(transform.position, _fire.transform.position) < _lightRange;
    }
    private bool IsPlayerSafe()
    {
        return Vector3.Distance(_player.transform.position, _fire.transform.position) <= _lightRange;
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
