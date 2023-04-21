using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class UmbraFSM : MonoBehaviour
{
    [SerializeField]
    Fire Fire;

    [SerializeField]
    Transform Player;

    [SerializeField]
    float ChillStateSpeed = 2;
    [SerializeField]
    float ChasingStateSpeed = 6;

    public Action OnCuteState;
    public Action OnChillState;
    public Action OnChasingState;

    public Vector2 PlayerOrientation => (Player.transform.position - transform.position).normalized;

    [SerializeField]
    UmbraStates _currentState = UmbraStates.Chill;

    private float CurrentRespectDistance => (_currentState != UmbraStates.Cute && _currentState == UmbraStates.Chill) ? Fire.LightRange + 5 : 1; 

    private void Awake()
    {
        ChangeState(_currentState);
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player && _currentState != UmbraStates.Cute)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void ChangeState(UmbraStates nextState)
    {
        Debug.Log("Umbra went from " + _currentState.ToString() 
            + " to " + nextState.ToString());
        _currentState = nextState;
        
        DoStateInvoke(_currentState);
    }

    private void DoStateInvoke(UmbraStates currentState)
    {
        switch (currentState)
        {
            case UmbraStates.Cute:
                OnCuteState?.Invoke();
                break;
            case UmbraStates.Chill:
                OnChillState?.Invoke();
                break;
            case UmbraStates.Chasing:
                OnChasingState?.Invoke();
                break;
            default:
                break;
        }
    }

    float GetStateSpeed(UmbraStates state)
    {
        switch (state)
        {
            case UmbraStates.Chill:
                return ChillStateSpeed;
            case UmbraStates.Chasing:
                return ChasingStateSpeed;
            default:
                return 0;
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case UmbraStates.Cute:
                UpdateCuteState();
                break;
            case UmbraStates.Chill:
                UpdateChillState();
                break;
            case UmbraStates.Chasing:
                UpdateChasingState();
                break;
            default:
                break;
        }
    }

    private float GetDistanceFromFire()
    {
        return Vector2.Distance(transform.position, Fire.transform.position);
    }
    private bool CanGoCuteState()
    {
        return GetDistanceFromFire() <= Fire.LightRange;
    }
    private bool CanGoChillState()
    {
        return Fire.IsAttached && GetDistanceFromFire() > Fire.LightRange && Fire.LightRange > 0;
    }
    private bool CanGoChasingState()
    {
        return !Fire.IsAttached && GetDistanceFromFire() > Fire.LightRange || Fire.LightRange <= 0;
    }

    void UpdateCuteState()
    {
        if (CanGoChillState())
        {
            ChangeState(UmbraStates.Chill);
            return;
        }
        else if (CanGoChasingState())
        {
            ChangeState(UmbraStates.Chasing);
            return;
        }
    }

    private float GetDistanceToPlayer()
    {
        return Vector2.Distance(transform.position, Player.transform.position);
    }

    void UpdateChillState()
    {
        float desiredSpeed = 2;
        
        if (GetDistanceToPlayer() > CurrentRespectDistance)
        {
            MoveTowardsPlayer(desiredSpeed);
        }

        if (CanGoCuteState())
        {
            ChangeState(UmbraStates.Cute);
            return;
        }
        else if (CanGoChasingState())
        {
            ChangeState(UmbraStates.Chasing);
        }
        
    }
    void UpdateChasingState()
    {
        float desiredSpeed = 5;
        
        if (GetDistanceToPlayer() > CurrentRespectDistance)
        {
            MoveTowardsPlayer(desiredSpeed);
        }

        if (CanGoCuteState())
        {
            ChangeState(UmbraStates.Cute);
            return;
        }
        else if (CanGoChillState())
        {
            ChangeState(UmbraStates.Chill);
            return;
        }
    }

    void MoveTowardsPlayer(float speed)
    {
        Vector2 playerDir = PlayerOrientation;
        Vector2 distanceToMove = playerDir * speed * Time.deltaTime;
        transform.Translate(distanceToMove);
    }
}


public enum UmbraStates
{
    Cute,
    Chill,
    Chasing,
}
