using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerController Player;
    [SerializeField] Animator Animator;

    private enum PlayerStates { Idle, Running, Jumping, Falling}

    private void Awake()
    {
        Player = GetComponent<PlayerController>();
    }
    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateFlipX()
    {
        var bodyScale = transform.localScale;
        bodyScale.x = Mathf.Abs(bodyScale.x) * Mathf.Sign(Player.XSpeed);
        transform.localScale = bodyScale;
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

        if (Player.YSpeed != 0)
        {
            if (Player.YSpeed > .1f)
                state = PlayerStates.Jumping;
            else
                state = PlayerStates.Falling;
        }

        Animator.SetInteger("state", (int)state);
    }
}
