using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Runner Player;
    Jumper _jumper;
    Thrower _thrower;
    [SerializeField] Animator Animator;
    [SerializeField] GameObject Body;
    

    private enum PlayerStates { Idle, Running, Jumping, Falling}

    private void OnEnable()
    {
        _jumper.OnSecondJump += OnSecondJump;
    }

    private void OnDisable()
    {
        _jumper.OnSecondJump -= OnSecondJump;
    }

    private void Awake()
    {
        Player = GetComponent<Runner>();
        _jumper = GetComponent<Jumper>();
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

        if (_jumper.YSpeed != 0)
        {
            if (_jumper.YSpeed > .1f)
                state = PlayerStates.Jumping;
            else
                state = PlayerStates.Falling;
        }

        Animator.SetBool("hasFire", _thrower.HasFire);
        Animator.SetInteger("state", (int)state);
    }

    private void OnSecondJump()
    {
        Animator.Rebind();
        Animator.Update(0f);
        Animator.SetInteger("state", 2);
    }
}
