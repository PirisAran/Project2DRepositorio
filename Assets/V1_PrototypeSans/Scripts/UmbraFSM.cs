using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class UmbraFSM : MonoBehaviour
{
    [SerializeField]
    FireController Fire;
    [SerializeField]
    PlayerController Player;
    [SerializeField]
    float TimeToRecover = 2f;
    [SerializeField]
    float ChillStateSpeed = 2f;
    [SerializeField]
    float ChasingStateSpeed = 6f;
    [SerializeField]
    float DistToSlowDown = 3;
    [SerializeField]
    float ChillStateRespDist = 5;
    [SerializeField]
    float ChaseStateRespDist = 1;

    public Action OnCuteState;
    public Action OnChillState;
    public Action OnChasingState;

    public Vector2 PlayerDirection => (Player.transform.position - transform.position).normalized;

    [SerializeField]
    UmbraStates _currentState = UmbraStates.Chill;

    private float CurrentRespectDistance => (_currentState != UmbraStates.Cute 
        && _currentState == UmbraStates.Chill) ? Fire.LightRange + ChillStateRespDist : ChaseStateRespDist; 

    private void Awake()
    {
        ChangeState(_currentState);
    }

    private void OnDrawGizmosSelected()
    {
        var playerPosition = Player.transform.position;

        if (_currentState != UmbraStates.Cute)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(playerPosition, CurrentRespectDistance);
        }
        if (_currentState == UmbraStates.Chill)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(playerPosition, DistToSlowDown + CurrentRespectDistance);
        }

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
        return IsInsideFire();
    }
    private bool CanGoChillState()
    {
        return Player.HasFire && GetDistanceFromFire() > Fire.LightRange && Fire.LightRange > 0;
    }
    private bool CanGoChasingState()
    {
        return !Player.HasFire && GetDistanceFromFire() > Fire.LightRange || Fire.LightRange <= 0;
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
        float maxSpeed = GetStateSpeed(_currentState);

        if (GetDistanceToPlayer() < CurrentRespectDistance)
        {
            BackUpFromFire(maxSpeed);
        }
        else
        {
            float distanceToRespectDistance = GetDistanceToPlayer() - CurrentRespectDistance;
            float currentSpeed = Mathf.Lerp(1, maxSpeed, Mathf.Clamp01(distanceToRespectDistance / DistToSlowDown ));
            Move(currentSpeed, PlayerDirection);
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

    private void BackUpFromFire(float speed)
    {
        Move(speed, -PlayerDirection);
    }

    private bool IsInsideFire()
    {
        return GetDistanceFromFire() <= Fire.LightRange;
    }

    void UpdateChasingState()
    {
        float maxSpeed = GetStateSpeed(_currentState);

        if (GetDistanceToPlayer() > CurrentRespectDistance)
        {
            Move(maxSpeed, PlayerDirection);
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

    void Move(float speed, Vector2 dir)
    {

        Vector2 distanceToMove = dir * speed * Time.deltaTime;
        if (distanceToMove.magnitude > GetDistanceToPlayer() + CurrentRespectDistance)
        {
             
        }
        transform.Translate(distanceToMove);
    }
}


public enum UmbraStates
{
    Cute,
    Chill,
    Chasing,
}
