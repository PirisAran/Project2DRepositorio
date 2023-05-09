using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    FireController Fire;

    //Components
    Rigidbody2D _rb;
    CollisionChecker _collCheck;

    //Movement
    float _horizontalMov;
    [SerializeField]
    float NoFireSpeed = 8, FireSpeed = 5 ;
    float _currentSpeed;

    //Jumping
    [SerializeField]
    float HighJumpHeight = 2.5f, LowJumpHeight = 1.0f;
    [SerializeField]
    float PressTimeToHighJump = 0.1f;
    float _jumpStartTime;
    [SerializeField]
    float MaxJumps = 2;
    float _multipleJumpsLeft;
    Vector2 _initialPosition;
    bool _isJumping = false;
    bool _isFirstJump = true;

    //Fire Throwing
    bool _hasFire = false;

    //Input
    [SerializeField]
    KeyCode JumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode ThrowKey = KeyCode.Mouse0;
    [SerializeField]
    KeyCode PickUpKey = KeyCode.E;

    private void OnEnable()
    {
        _collCheck.OnLanding += OnLanding;
    }
    private void OnDisable()
    {
        _collCheck.OnLanding -= OnLanding;
    }
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collCheck = GetComponent<CollisionChecker>();
        _currentSpeed = FireSpeed;
        ResetJumps();
    }

    private void Update()
    {
        CheckInputs();  
        UpdateMove();
        UpdateJump();
    }

    private void CheckInputs()
    {
        JumpInput();
        //ThrowInput();
        MoveInput();
        //PickUpInput();
    }

    /* ----------------- PLAYER MOVEMENT AND JUMPING --------------------- */
    
    // ---- MOVEMENT ---- //
    private void MoveInput()
    {
        _horizontalMov = Input.GetAxis("Horizontal");
    }
    private void UpdateMove()
    {
        Move();
    }
    private void Move()
    {
        var vel = new Vector2(_horizontalMov * _currentSpeed, _rb.velocity.y);
        _rb.velocity = vel;
    }


    // ---- JUMPING ---- //
    private void UpdateJump()
    {
        //Comprueba si tiene el fuego y si esta saltando, que son las condiciones para poder hacer el salto alto
        if (!_hasFire && _isJumping)
            TryAddExtraJumpForce();
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(JumpKey))
            JumpStarted();
        if (Input.GetKeyUp(JumpKey))
            JumpFinished();
    }
    private bool OnGround()
    {
        return _collCheck.OnGround;
    }
    
    private void JumpStarted()
    {
        if (!_hasFire)
        {
            DoMultipleJump();
            return;
        }

        if (OnGround())
            Jump();
    }

    //SALTO SIN EL FUEGO
    private void DoMultipleJump()
    {
        if (_isFirstJump)
        {
            if (!OnGround())
            {
                _multipleJumpsLeft--;
                _isFirstJump = false;
            }
        }

        if (_multipleJumpsLeft > 0)
        {
            _multipleJumpsLeft--;
            Jump();
        }
    }

    private void Jump()
    {
        _isJumping = true;
        _jumpStartTime = Time.time;
        AddJumpForce(LowJumpHeight);
    }

    // SOLO SE HACE ESTO CUANDO NO TIENE EL FUEGO
    private void TryAddExtraJumpForce()
    {
        if (Time.time - _jumpStartTime >= PressTimeToHighJump && _isFirstJump)
        {
            var currentPosition = transform.position;
            //Le pasamos la altura que le queda para llegar a la HighJumpHeight
            AddJumpForce(HighJumpHeight - Vector2.Distance(currentPosition, _initialPosition));
            _isFirstJump = false;
        }
    }

    //Añadir fuerza al jugador para que salte el jugador con altura deseada. Se llama al pulsar el SPACE // SOLO SIN EL FUEGOOO
    private void AddJumpForce(float heightToReach)
    {
        _initialPosition = transform.position;
        _rb.velocity = new Vector2(_rb.velocity.x, GetJumpForce(heightToReach));
    }

    //Pillar la fuerza del salto
    private float GetJumpForce(float height)
    {
        return Mathf.Sqrt(2 * Physics.gravity.magnitude * height);
    }

    private void JumpFinished()
    {
        //Se acaba el salto
        _isJumping = false;
    }

    private void OnLanding()
    {
        ResetJumps();
        _isFirstJump = true;
    }
    private void ResetJumps()
    {
        _multipleJumpsLeft = MaxJumps;
    }


    /* ------------- FIRE THROWER CONTROLLER ---------- */
    private void PickUpInput()
    {
        if (Input.GetKeyDown(PickUpKey))
        {
            TryPickUp();
        }
    }

    //Try pick uf the fire
    private void TryPickUp()
    {

    }
    private void ThrowInput()
    {
        if (Input.GetKeyDown(ThrowKey))
            ThrowFireStart();

        if (Input.GetKeyUp(ThrowKey))
            ThrowFireFinish();
    }

    private void ThrowFireFinish()
    {
        throw new NotImplementedException();
    }

    private void ThrowFireStart()
    {

    }

}
