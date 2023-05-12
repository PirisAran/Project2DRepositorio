using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    [SerializeField]
    PlayerController _player;
    FireController _fire;

    [SerializeField]
    float VelocityReduction = 0.5f, JumpReduction = 0.5f;

    float _lastTimeDamage;

    [SerializeField]
    float WaterDamageRate = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
        {
            _lastTimeDamage = Time.time;
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
            _player.ChangeSpeeds(VelocityReduction);
            _player.ChangeJumps(JumpReduction);
        }
        else
        {
            Debug.Log("speeds and jumps RESET");
            _player.ResetValues();
        }
    }
    
        
    private void WaterHurting()
    {
        if (Time.time - _lastTimeDamage >= WaterDamageRate)
        {
            //_fire.CurrentFireHealth -= DamageWaterRate;
            Debug.Log("-1 Fire Life");
        }
    }
}
