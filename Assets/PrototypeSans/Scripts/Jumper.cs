using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    PlayerInput _playerInput;
    CollisionChecker _collisionCheck;

    [SerializeField]
    private float JumpHeight = 2.5f;

    [SerializeField]
    private float TimeToPeak = 1f;

    [SerializeField]
    private float PressTimeToMaxJump = 2f;

    [SerializeField]
    private bool _multipleJumpAllowed = true;
    public bool MultipleJumpAllowed => _multipleJumpAllowed;


    float _jumpStartTime;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionChecker>();
    }

    private void OnEnable()
    {
        _playerInput.OnJumpStarted += OnJumpStarted;
        _playerInput.OnJumpFinished += OnJumpFinished;
    }

    private void OnDisable()
    {
        _playerInput.OnJumpStarted -= OnJumpStarted;
        _playerInput.OnJumpFinished -= OnJumpFinished;
    }

    private void SetGravity()
    {
        var gravDesired = 2 * JumpHeight / (TimeToPeak * TimeToPeak);
        _rigidbody2D.gravityScale = (gravDesired / Vector3.Magnitude(Physics.gravity)) * Math.Sign(_rigidbody2D.gravityScale);
    }

    private float GetJumpForce()
    {
        return (2 * JumpHeight / TimeToPeak) * Math.Sign(_rigidbody2D.gravityScale);
    }

    private void OnJumpStarted()
    {
        if (OnGround())
            Jump();
    }

    private void OnJumpFinished()
    {
        if (PressTimeToMaxJump > TimeToPeak) PressTimeToMaxJump = TimeToPeak;

        var timePassed = Time.time - _jumpStartTime;
        var proportionTimePassed = Mathf.Clamp01((timePassed / PressTimeToMaxJump));

        _rigidbody2D.gravityScale *= (1 / proportionTimePassed);
    }

    private void Jump()
    {
        SetGravity();
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, GetJumpForce());
        _jumpStartTime = Time.time;
    }

   

    private bool OnGround()
    {
        return  _collisionCheck.CanJump;
    }
}
