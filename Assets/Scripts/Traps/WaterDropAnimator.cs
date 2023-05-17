using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropAnimator : MonoBehaviour
{
    Animator _animator;
    Rigidbody2D _rb;



    private enum States { Falling, Breaking}
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        States state;

        if (_rb.velocity.y != 0)
        {
            state = States.Falling;
        }
        else
        {
            state = States.Breaking;
        }

        _animator.SetInteger("state", (int)state);
    }
}
