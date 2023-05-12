using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTrap : MonoBehaviour
{
    [SerializeField]
    GameObject Player;
    FireController _fire;

    [SerializeField]
    float VelocityReduction = 0.5f, JumpReduction = 0.5f;

    float _lastTimeDamage;

    [SerializeField]
    float WaterDamageRate = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player.transform)
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
        }
        else
        {
            Debug.Log("speeds and jumps RESET");
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
