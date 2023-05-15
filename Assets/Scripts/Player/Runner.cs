using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{

    //Components
    Rigidbody2D _rb;
    Thrower _thrower;

    //Movement
    float _horizontalMov;
    [SerializeField]
    float NoFireSpeed = 8, FireSpeed = 5;
    float _currentStateSpeed;
    public Vector2 Forward => new Vector2(_horizontalMov, 0).normalized;
    public float XSpeed { get; private set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentStateSpeed = FireSpeed;
        _thrower = GetComponent<Thrower>();
    }

    private void Update()
    {
        MoveInput();
        UpdateMove();
    }

    private void MoveInput()
    {
        _horizontalMov = Input.GetAxisRaw("Horizontal");
    }
    private void UpdateMove()
    {
        Move();
    }
    private void Move()
    {
        var vel = new Vector2(_horizontalMov * _currentStateSpeed, _rb.velocity.y);
        _rb.velocity = vel;
        XSpeed = vel.x;
    }

    public void ChangeSpeed()
    {
        _currentStateSpeed = _thrower.HasFire ? FireSpeed : NoFireSpeed;
    }

}
   
    
