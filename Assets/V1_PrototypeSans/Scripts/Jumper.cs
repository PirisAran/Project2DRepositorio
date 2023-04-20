using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    PlayerInput _playerInput;
    CollisionChecker _collisionCheck;
    FireThrower _fireThrower;

    [SerializeField]
    private float JumpHeight = 2.5f;

    [SerializeField]
    private float TimeToPeak = 1f;

    [SerializeField]
    private float PressTimeToMaxJump = 2f;

    [SerializeField]
    bool MultipleJumpActive = true;

    [SerializeField]
    float MaxJumpsNum = 2;

    float _jumpsLeft;

    bool _firstJump = true;
    
    [SerializeField]
    float _gravityTweak = 3;


    float _jumpStartTime;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collisionCheck = GetComponent<CollisionChecker>();
        _fireThrower = GetComponent<FireThrower>();
        ResetJumps();
    }

    private void OnEnable()
    {
        _playerInput.OnJumpStarted += OnJumpStarted;
        _playerInput.OnJumpFinished += OnJumpFinished;
        _collisionCheck.OnLanding += OnLanding;
        _playerInput.OnThrowFinished += OnThrowFinished;
        _fireThrower.OnFirePickedUp += OnFirePickedUp;
    }

    private void OnDisable()
    {
        _playerInput.OnJumpStarted -= OnJumpStarted;
        _playerInput.OnJumpFinished -= OnJumpFinished;
        _collisionCheck.OnLanding -= OnLanding;
        _playerInput.OnThrowFinished -= OnThrowFinished;
        _fireThrower.OnFirePickedUp -= OnFirePickedUp;
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
        TryJump();
    }

    void TryJump()
    {
        if (MultipleJumpActive)
        {
            MultipleJump();
            return;
        }

        if (OnGround())
            Jump();
    }

    void MultipleJump()
    {
        if (_firstJump)
        {
            if (!OnGround())
            {
                _jumpsLeft--;
            }
            DoMultipleJump();
            _firstJump = false;
            return;
        }
        
        if (_jumpsLeft > 0)
        {
            DoMultipleJump();
        }
    }

    void DoMultipleJump()
    {
        Jump();
        _jumpsLeft--;
    }

    private void OnJumpFinished()
    {
        if (PressTimeToMaxJump > TimeToPeak) PressTimeToMaxJump = TimeToPeak;

        var timePassed = Time.time - _jumpStartTime;
        var proportionTimePassed = Mathf.Clamp01((timePassed / PressTimeToMaxJump));

        _rigidbody2D.gravityScale *= (1 / proportionTimePassed);
        TweakGravity();
    }

    private void TweakGravity()
    {
        _rigidbody2D.gravityScale += _gravityTweak;
    }

    private void Jump()
    {
        SetGravity();
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, GetJumpForce());
        _jumpStartTime = Time.time;
    }

    private bool OnGround()
    {
        return  _collisionCheck.OnGround;
    }

    void OnLanding()
    {
        ResetJumps();
    }

    void ResetJumps()
    {
        _jumpsLeft = MaxJumpsNum;
    }

    void OnThrowFinished()
    {
        MultipleJumpActive = true;
    }

    void OnFirePickedUp()
    {
        MultipleJumpActive = false;
    }
}
