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
    [SerializeField]
    UmbraStates CurrentState;
    UmbraStates _desiredState;
    [SerializeField]
    float ChasingSpeed = 2.0f, KillerSpeed = 4.0f;
    [SerializeField]
    float ToCuteTimer = 0.5f, FromCuteTimer = 1.5f;
    float _changeTimer;
    //Actions
    public Action OnCuteState;
    public Action OnChasingState;
    public Action OnKillerState;

    Vector2 _fireDir => (Fire.transform.position - transform.position).normalized;
    Vector2 _playerDir => (Player.transform.position - transform.position).normalized;


    // Start is called before the first frame update
    void Start()
    {
        CurrentState = UmbraStates.Chasing;
        OnEnterState(CurrentState);
    }

    private void ChangeState(UmbraStates nextState)
    {
        Debug.Log("Changed IS CHANGING from " + CurrentState + " to " + nextState);
        CurrentState = UmbraStates.ChangingState;
        _desiredState = nextState;
        OnChangingState();
    }
    private void OnEnterState(UmbraStates state)
    {
        switch (state)
        {
            case UmbraStates.Cute:
                OnCuteState?.Invoke();
                break;
            case UmbraStates.Chasing:
                OnChasingState?.Invoke();
                break;
            case UmbraStates.Killer:
                OnKillerState?.Invoke();
                break;
            case UmbraStates.ChangingState:
                break;
            default:
                break;
        }
    }

    private void OnChangingState()
    {
        switch (_desiredState)
        {
            case UmbraStates.Cute:
                break;
            case UmbraStates.Chasing:
                break;
            case UmbraStates.Killer:
                break;
            default:
                break;
        }

    }

    void Update()
    {
        switch (CurrentState)
        {
            case UmbraStates.ChangingState:
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
            CurrentState = _desiredState;
            OnEnterState(CurrentState);
        }
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
    }
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
}

public enum UmbraStates
{
    Cute,                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    Chasing,
    Killer,
    ChangingState
}
