using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Runner _velocity;

    [SerializeField]
    Jumper _jump; 

    [SerializeField]
    float VelocityReduction = 0.5f, JumpReduction = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player.transform)
        {
            ApplyEffect(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ApplyEffect(false);
    }

    private void ApplyEffect(bool v)
    {
        if (v)
        {
            Debug.Log("speeds and jumps CHANGED");
            SetNewSpeedAndJump();
        }
        else
        {
            Debug.Log("speeds and jumps RESET");
            _velocity.ChangeSpeed();
            _jump.SetPreviousJumps();
        }
    }

    private void SetNewSpeedAndJump()
    {
        _velocity.CurrentSpeed *= VelocityReduction;
        _jump.HighJump *= JumpReduction;
        _jump.LowJump *= JumpReduction;
    }


}
