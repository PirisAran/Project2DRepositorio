using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool IsMoving => _isMoving;

    [SerializeField]
    private float SpeedWithFire = 5;

    [SerializeField]
    private float SpeedNoFire = 8;

    float _currentSpeed;

    private bool _isMoving;

    PlayerInput _input;
    Rigidbody2D _rigidbody;
    FireThrower _fireThrower;

    void OnEnable()
    {
        _input.OnThrowFinished += OnThrowFinished;
        _fireThrower.OnFirePickedUp += OnFirePickedUp;
    }

    void OnDisable()
    {
        _input.OnThrowFinished -= OnThrowFinished;
        _fireThrower.OnFirePickedUp -= OnFirePickedUp;
    }

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _fireThrower = GetComponent<FireThrower>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction = new Vector2(_input.MovementHorizontal * _currentSpeed, _rigidbody.velocity.y) ;

        _rigidbody.velocity = direction;
        _isMoving = direction.magnitude > 0.01f;
    }

    void OnThrowFinished()
    {
        _currentSpeed = SpeedNoFire;
    }

    void OnFirePickedUp()
    {
        _currentSpeed = SpeedWithFire;
    }
}
