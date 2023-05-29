using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Runner Player;
    Jumper jump;
    Thrower _thrower;
    [SerializeField] Animator Animator;
    [SerializeField] GameObject Body;
    

    private enum PlayerStates { Idle, Running, Jumping, Falling}

    private void Awake()
    {
        Player = GetComponent<Runner>();
        jump = GetComponent<Jumper>();
        _thrower = GetComponent<Thrower>();
    }
    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateFlipX()
    {
        var bodyScale = Body.transform.localScale;
        bodyScale.x = Mathf.Abs(bodyScale.x) * Mathf.Sign(Player.XSpeed);
        Body.transform.localScale = bodyScale;
    }

    private void UpdateAnimationState()
    {
        PlayerStates state;

        if (Player.XSpeed > 0)
        {
            state = PlayerStates.Running;
            UpdateFlipX();
        }
        else if (Player.XSpeed < 0)
        {
            state = PlayerStates.Running;
            UpdateFlipX();
        }
        else state = PlayerStates.Idle;

        if (jump.YSpeed != 0)
        {
            if (jump.YSpeed > .1f)
                state = PlayerStates.Jumping;
            else
                state = PlayerStates.Falling;
        }

        Animator.SetBool("hasFire", _thrower.HasFire);
        Animator.SetInteger("state", (int)state);
    }
}
