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
    V2UmbraStates CurrentState;
    [SerializeField]
    float ChasingSpeed = 2.0f;
    [SerializeField]
    float KillerSpeed = 4.0f;
    public Action OnHarmlessState;
    public Action OnChasingState;
    public Action OnKillerState;

    Vector2 _fireDir => (Fire.transform.position - transform.position).normalized;
    Vector2 _playerDir => (Player.transform.position - transform.position).normalized;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void ChangeState(V2UmbraStates nextState)
    {
        CurrentState = nextState;
        DoStateInvoke(CurrentState);
    }
    private void DoStateInvoke(V2UmbraStates state)
    {
        switch (state)
        {
            case V2UmbraStates.Harmless:
                OnHarmlessState?.Invoke();
                break;
            case V2UmbraStates.Chasing:
                OnChasingState?.Invoke();
                break;
            case V2UmbraStates.Killer:
                OnKillerState?.Invoke();
                break;
            default:
                break;
        }
    }
    void Update()
    {
        switch (CurrentState)
        {
            case V2UmbraStates.Harmless:
                UpdateHarmless();
                break;
            case V2UmbraStates.Chasing:
                UpdateChasing();
                break;
            case V2UmbraStates.Killer:
                UpdateKiller();
                break;
            default:
                break;
        }
    }
    private void UpdateHarmless()
    {

        if (UmbraIsLit())
            return;
        if (PlayerIsSafe())
            ChangeState(V2UmbraStates.Chasing);
        else
            ChangeState(V2UmbraStates.Killer);
    }
    private void UpdateChasing()
    {
        if (UmbraIsLit())
            ChangeState(V2UmbraStates.Harmless);
    }
    private void UpdateKiller()
    {
        if (UmbraIsLit())
            ChangeState(V2UmbraStates.Harmless);
    }

    private void RunFromFire()
    {
        //Move(-_fireDir, 1);
    }

    private void Move(Vector2 dir, float speed)
    {
        Vector2 disToMove = dir * speed * Time.deltaTime;
        transform.Translate(disToMove);
    }

    private bool PlayerIsSafe()
    {
        return Vector2.Distance(Player.transform.position, Fire.transform.position) < Fire.LightRange;
    }
    private bool UmbraIsLit()
    {
        return Vector2.Distance(transform.position, Fire.transform.position) < Fire.LightRange;
    }
}

public enum V2UmbraStates
{
    Harmless,                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    Chasing,
    Killer
}
