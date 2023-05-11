using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbraController : MonoBehaviour
{
    [SerializeField]
    PlayerController Player;
    [SerializeField]
    FireController Fire;

    // FSM
    [SerializeField] UmbraStates CurrentState;
    UmbraStates _nextState;
    UmbraStates _previousState;
    [SerializeField] float CuteSpeed = 1.0f, ChasingSpeed = 2.0f, KillerSpeed = 4.0f;
    [SerializeField] float FromCuteTime = 1.5f, ToCuteTime = 0.5f, ToChasingTime = 0.5f, ToKillerTime = 0.75f ;
    float _changeTimer;
    [SerializeField] float AddedChasingDistance = 3.0f;
    [SerializeField] float ChaseZoneOffset = 0.5f;
    [SerializeField] float DistToAccelerate = 6;
    //Movement
    [SerializeField] float CuteAcceleration = 2.0f, ChasingAcceleration = 2.0f, KillerAcceleration = 5.0f;
    float _currentDeceleration;
    float _deltaTime;
    float _currentSpeed;
    Vector2 _direction;
    //Actions
    public Action OnCuteState;
    public Action OnChasingState;
    public Action OnKillerState;
    public Action OnChangingState;

    Vector2 _fireDir => (Fire.transform.position - transform.position).normalized;
    Vector2 _playerDir => (Player.transform.position - transform.position).normalized;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Fire.transform.position, Fire.LightRange);

    }


    // Start is called before the first frame update
    void Start()
    {
        CurrentState = UmbraStates.Chasing;
        OnEnterState(CurrentState);
    }
    private void ChangeState(UmbraStates nextState)
    {
        _previousState = CurrentState;
        CurrentState = UmbraStates.Changing;
        _nextState = nextState;
        InitChangingState();
    }
    private void OnEnterState(UmbraStates state)
    {
        switch (state)
        {
            case UmbraStates.Cute:
                OnCuteState?.Invoke();
                _currentSpeed = 0;
                break;
            case UmbraStates.Chasing:
                OnChasingState?.Invoke();
                break;
            case UmbraStates.Killer:
                OnKillerState?.Invoke();
                break;
            case UmbraStates.Changing:
                break;
            default:
                break;
        }
    }

    private void InitChangingState()
    {
        OnChangingState?.Invoke();
        if (_previousState == UmbraStates.Cute)
        {
            _changeTimer = FromCuteTime;
            SetDeceleration();
            return;
        }

        switch (_nextState)
        {
            case UmbraStates.Cute:
                _changeTimer = ToCuteTime;
                break;
            case UmbraStates.Chasing:
                _changeTimer = ToChasingTime;
                break;
            case UmbraStates.Killer:
                _changeTimer = ToKillerTime;
                break;
            case UmbraStates.Changing:
                break;
            default:
                break;
        }
        SetDeceleration();
    }

    private void SetDeceleration()
    {
        _deltaTime = _changeTimer;
        float deltaSpeed = 0 - _currentSpeed;
        _currentDeceleration = deltaSpeed / _deltaTime;
    }

    void Update()
    {
        switch (CurrentState)
        {
            case UmbraStates.Changing:
                UpdateChangingState();
                break;
            case UmbraStates.Cute:
                UpdateCute();
                break;
            case UmbraStates.Chasing:
                UpdateChasing();
                break;
            case UmbraStates.Killer:
                UpdateKiller();
                break;
            default:
                break;
        }
        MoveUmbra();
    }

    private void UpdateChangingState()
    {
        _changeTimer -= Time.deltaTime;
        if (_changeTimer <= 0)
        {
            CurrentState = _nextState;
            OnEnterState(CurrentState);
        }

        //Mientras cambia, va decelerando hasta alcanzar 0 SIEMPRE. Asi, entre estados el umbra frena hasta llegar a 0;
        if (_currentSpeed > 0)
            _currentSpeed = Mathf.Clamp(_currentSpeed + _currentDeceleration * Time.deltaTime, 0, 999); 
    }

    private void UpdateCute()
    {
        if (CanTurnChasing())
        {
            ChangeState(UmbraStates.Chasing);
            return;
        }
        if (CanTurnKiller())
        {
            ChangeState(UmbraStates.Killer);
            return;
        }

        _direction = -_fireDir;

        AdjustSpeed();

        //if (_currentSpeed < GetMaxSpeedState(CurrentState))
        //{
        //    _currentSpeed = Mathf.Clamp(_currentSpeed + Acceleration * Time.deltaTime, 0, CuteSpeed);
        //}
        //else if (_currentSpeed > GetMaxSpeedState(CurrentState))
        //{
        //    _currentSpeed = Mathf.Clamp(_currentSpeed - Acceleration * Time.deltaTime, CuteSpeed, 999);
        //}
    }
    private void UpdateChasing()
    {
        if (CanTurnCute())
        {
            ChangeState(UmbraStates.Cute);
            return;
        }
        if (CanTurnKiller())
        {
            ChangeState(UmbraStates.Killer);
            return;
        }

        //Se mueve a distancia del jugador

        float maxSpeed = GetMaxSpeedState(CurrentState);
        _direction = _playerDir;
        float distanceToPlayer = ToPlayerDist();
        float decelerateZoneRadius = DistToAccelerate + Fire.LightRange;
        float respectDistance = AddedChasingDistance + Fire.LightRange;

        if (distanceToPlayer > decelerateZoneRadius)
        {
            float distFraction = Mathf.Clamp01(distanceToPlayer / distanceToPlayer - respectDistance);
            //_currentSpeed = Mathf.Lerp(maxSpeed, Player.XSpeed,  );
        }
        else if (distanceToPlayer > respectDistance)
        {

        }


       


        //AdjustSpeed();

        //float speed;
        //if (_currentSpeed < ChasingSpeed)
        //{
        //    speed = Mathf.Clamp(_currentSpeed + Acceleration * Time.deltaTime, 0, ChasingSpeed);
        //}
        //else
        //{
        //    speed = Mathf.Clamp(_currentSpeed - Acceleration * Time.deltaTime, ChasingSpeed, 999);
        //}    

    }
    private void UpdateKiller()
    {
        if (CanTurnCute())
        {
            ChangeState(UmbraStates.Cute);
            return;
        }
        if (CanTurnChasing())
        {
            ChangeState(UmbraStates.Chasing);
            return;
        }

        //Va directo al jugador
        _direction = _playerDir;

        AdjustSpeed();

        //_currentSpeed = KillerSpeed;
        //if (_currentSpeed < KillerSpeed)
        //{
        //    _currentSpeed = Mathf.Clamp(_currentSpeed + Acceleration * Time.deltaTime, 0, KillerSpeed);
        //}
    }

    private void MoveUmbra()
    {
        Vector2 distance = _direction * _currentSpeed * Time.deltaTime;
        transform.Translate(distance);
    }

    //Comprovadores del estado de player y de umbra
    private bool PlayerIsSafe()
    {
        //Comprueva si la distancia del player respecto el fuego es mayor o menor q el rango de la luz. 
        return Vector2.Distance(Player.transform.position, Fire.transform.position) < Fire.LightRange;
    }
    private bool UmbraIsLit()
    {
        //Comprueva si la distancia del Umbra respecto el fuego y mira si el umbra esta dentro del rango de luz.
        return Vector2.Distance(transform.position, Fire.transform.position) < Fire.LightRange;
    }

    //Comprovadores de si se puede cambiar a un estado o no
    private bool CanTurnCute()
    {
        return UmbraIsLit();
    }
    private bool CanTurnChasing()
    {
        return !UmbraIsLit() && PlayerIsSafe();
    }
    private bool CanTurnKiller()
    {
        return !UmbraIsLit() && !PlayerIsSafe();
    }

    //Comprovadores de distancia
    private float ToPlayerDist()
    {
        return Vector2.Distance(transform.position, Player.transform.position);
    }
    private float ToFireDist()
    {
        return Vector2.Distance(transform.position, Fire.transform.position);
    }

    private float GetMaxSpeedState(UmbraStates state)
    {
        float speed = 0;

        switch (state)
        {
            case UmbraStates.Cute:
                speed = CuteSpeed;
                break;
            case UmbraStates.Chasing:
                speed = ChasingSpeed;
                break;
            case UmbraStates.Killer:
                speed = KillerSpeed;
                break;
            default:
                break;
        }
        Debug.Log("STATE SPEED " + speed);
        return speed;
    }
    private float GetAccelerationState (UmbraStates state)
    {
        float acc = 0;

        switch (state)
        {
            case UmbraStates.Cute:
                acc = CuteAcceleration;
                break;
            case UmbraStates.Chasing:
                acc = ChasingAcceleration;
                break;
            case UmbraStates.Killer:
                acc = KillerAcceleration;
                break;
            default:
                break;
        }
        return acc;
    }
    private void AdjustSpeed()
    {
        float maxSpeed = GetMaxSpeedState(CurrentState);
        float acc = GetAccelerationState(CurrentState);

        if (_currentSpeed < maxSpeed)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + acc * Time.deltaTime, 0, maxSpeed);
        }
        else if (_currentSpeed > maxSpeed)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed - acc * Time.deltaTime, maxSpeed, 0);
        }
    }

}

public enum UmbraStates
{
    Cute,                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    Chasing,
    Killer,
    Changing
}
