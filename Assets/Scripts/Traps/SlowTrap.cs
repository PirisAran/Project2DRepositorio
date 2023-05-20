using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class SlowTrap : MonoBehaviour
{
    [SerializeField] Transform _player;

    Runner _velocity;

    Jumper _jump;

    [SerializeField]
    float VelocityReduction = 0.5f, JumpReduction = 0.5f;

    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        _velocity = _player.GetComponent<Runner>();
        _jump = _player.GetComponent<Jumper>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
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
