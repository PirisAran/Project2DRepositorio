using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UmbraController : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    [SerializeField]
    FireController Fire;
    Runner _playerRunner;

    // FSM
    [SerializeField] 
    UmbraStates m_currentState;
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

    private void Awake()
    {
        _playerRunner = Player.GetComponent<Runner>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Fire.transform.position, Fire.LightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Fire.transform.position, Fire.LightRange + AddedChasingDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(Fire.transform.position, Fire.LightRange + DistToAccelerate);
    }
    
    void Start()
    {
        m_currentState = UmbraStates.Chasing;
        OnEnterState(m_currentState);
    }
    private void ChangeState(UmbraStates nextState)
    {
        _previousState = m_currentState;
        m_currentState = UmbraStates.Changing;
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
        switch (m_currentState)
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
    }

    private void UpdateChangingState()
    {
        _changeTimer -= Time.deltaTime;
        if (_changeTimer <= 0)
        {
            m_currentState = _nextState;
            OnEnterState(m_currentState);
        }

        //Mientras cambia, va decelerando hasta alcanzar 0 SIEMPRE. Asi, entre estados el umbra frena hasta llegar a 0;
        if (_currentSpeed > 0)
            _currentSpeed = Mathf.Clamp(_currentSpeed + _currentDeceleration * Time.deltaTime, 0, 999);
        MoveUmbra();

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
        MoveUmbra();

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

        float maxSpeed = GetMaxSpeedState(m_currentState);
        _direction = _playerDir;
        float distanceToPlayer = ToFireDist();
        float decelerateZoneRadius = DistToAccelerate + Fire.LightRange;
        float respectDistance = AddedChasingDistance + Fire.LightRange;
        float playerSpeed = Math.Abs(_playerRunner.XSpeed);

        if (distanceToPlayer > decelerateZoneRadius)
        {
            _currentSpeed = maxSpeed;
            _direction = _playerDir;
        }
        else if (distanceToPlayer > respectDistance)
        {
            float maxDistanceFraction = decelerateZoneRadius - respectDistance;
            float currentDistance = distanceToPlayer - respectDistance;
            _currentSpeed = Mathf.Lerp(maxSpeed, playerSpeed, Mathf.Clamp01(currentDistance / maxDistanceFraction));
            _direction = _playerDir;

            float mov = _currentSpeed * Time.deltaTime;
            if (mov > currentDistance)
                mov = currentDistance;
            _currentSpeed = mov / Time.deltaTime;
            
        }
        else if (distanceToPlayer < respectDistance)
        {
            float maxDistanceFraction = respectDistance - Fire.LightRange;
            float currentDistance = distanceToPlayer - Fire.LightRange;
            _currentSpeed = Mathf.Lerp(playerSpeed, maxSpeed, currentDistance / maxDistanceFraction);
            _direction = -_fireDir;
            float mov = _currentSpeed * Time.deltaTime;
            if (mov > currentDistance)
                mov = currentDistance;
            _currentSpeed = mov / Time.deltaTime;
        }
        MoveUmbra();
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
        MoveUmbra();
        
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
        float maxSpeed = GetMaxSpeedState(m_currentState);
        float acc = GetAccelerationState(m_currentState);

        if (_currentSpeed < maxSpeed)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed + acc * Time.deltaTime, 0, maxSpeed);
        }
        else if (_currentSpeed > maxSpeed)
        {
            _currentSpeed = Mathf.Clamp(_currentSpeed - acc * Time.deltaTime, maxSpeed, 0);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_currentState == UmbraStates.Cute || m_currentState == UmbraStates.Changing)
            return;

        if (collision.transform == Player.transform)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

public enum UmbraStates
{
    Cute,                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    Chasing,
    Killer,
    Changing
}
