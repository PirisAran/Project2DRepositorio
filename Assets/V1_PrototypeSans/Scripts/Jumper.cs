using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    //Components
    Rigidbody2D _rigidbody2D;
    PlayerInput _playerInput;
    CollisionChecker _collisionCheck;
    FireThrower _fireThrower;

    bool _hasFire = true;
    bool _firstAddedForce = true;

    //Jump
    [SerializeField]
    private float HighJumpHeight = 2.5f;
    [SerializeField]
    private float LowJumpHeight = 1.0f;
    [SerializeField]
    private float PressTimeToHighJump = 0.3f;
    float _jumpStartTime;
    Vector2 _initialPosition;
    bool _isJumping = false;

    //Multiple Jump
    bool _firstJump = true;
    float _multipleJumpsLeft;
    [SerializeField]
    float MaxMultipleJumps = 5;

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

    private void Update()
    {
        if (_isJumping && !_hasFire)
            TryAddExtraJumpForce();
    }

    private void OnJumpStarted()
    {
        TryJump();
    }
    void TryJump()
    {
        if (!_hasFire)
        {
            DoMultipleJump();
            return;
        }

        if (OnGround())
        {
            DoJump();
            _jumpStartTime = Time.time;
        }
    }

    private void DoMultipleJump()
    {
        if (_firstJump)
        {
            if (!OnGround())
            {
                _firstAddedForce = false;
                _multipleJumpsLeft--;
            }
            _firstJump = false;
        }

        if (_multipleJumpsLeft > 0)
        {
            _multipleJumpsLeft--;
            DoJump();
        }
    }
    private void DoJump()
    {
        _isJumping = true;
        Debug.Log("NormalJump");
        AddVerticalForce(LowJumpHeight);
        _jumpStartTime = Time.time;
    }
    private void AddVerticalForce(float height)
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, GetJumpForce(height));
        _initialPosition = transform.position;
    }
    private void TryAddExtraJumpForce()
    {
        if (Time.time - _jumpStartTime >= PressTimeToHighJump && _firstAddedForce)
        {
            var currentPosition = transform.position;
            AddVerticalForce(HighJumpHeight - Vector2.Distance(currentPosition, _initialPosition));
            _isJumping = false;
            _firstAddedForce = false;
        }
    }
    private float GetJumpForce(float height)
    {
        return Mathf.Sqrt(2*Physics.gravity.magnitude * height);
    }
    private bool OnGround()
    {
        return  _collisionCheck.OnGround;
    }

    private void OnJumpFinished()
    {
        _isJumping = false;
    }

    void ResetJumps()
    {
        _multipleJumpsLeft = MaxMultipleJumps;
    }

    void OnLanding()
    {
        _firstJump = true;
        _firstAddedForce = true;
        ResetJumps();
    }

    void OnThrowFinished()
    {
        _hasFire = false;
    }

    void OnFirePickedUp()
    {
        _hasFire = true;
    }
}
