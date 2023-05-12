using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
                                          /* ------------------PLAYER CONTROLLER SCRIPT---------------------*/         
    [SerializeField]
    FireController Fire;


    //Components
    Rigidbody2D _rb;
    CollisionChecker _collCheck;
    LineRenderer _lr;

    //Movement
    float _horizontalMov;
    [SerializeField]
    float NoFireSpeed = 8, FireSpeed = 5 ;
    float _currentSpeed;
    public Vector2 Forward => new Vector2(_horizontalMov, 0).normalized;


    //Jumping------------------------
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
    bool _firstAddedForce = true;

    //Fire Throwing and picking----------------------
    [SerializeField]
    public Collider2D PickUpCollider;
    public bool HasFire => _hasFire;
    bool _hasFire = true;
    [SerializeField]
    float MinThrowSpeed = 2, MaxThrowSpeed = 20;
    [SerializeField]
    float TimeMaxThrow = 0.5f;
    //[SerializeField]
    //float DeltaSpeed = 3.0f;
    //[SerializeField]
    //float ParabolicShootAngle = 45.0f;
    [SerializeField]
    int ParabolicShootMaxPoints = 200;
    [SerializeField]
    float ParabolicShootTime = 4.0f;
    float _throwStartTime;
    public bool IsChargingThrow => _isChargingThrow;
    bool _isChargingThrow = false;

    //Input------------------------
    [SerializeField]
    KeyCode JumpKey = KeyCode.Space;
    [SerializeField]
    KeyCode ThrowKey = KeyCode.Mouse0;
    [SerializeField]
    KeyCode PickUpKey = KeyCode.E;
    [SerializeField]
    KeyCode CancelThrowKey = KeyCode.Mouse1;
    
    //Provisonal Variables
    float previousFireSpeed;
    float previousNoFireSpeed;
    float previousLowJump;
    float previousHighJump;

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
        _lr = GetComponent<LineRenderer>();
        _currentSpeed = FireSpeed;
        ResetJumps();
        PreviousValues();
    }


    private void Update()
    {
        CheckInputs();  
        UpdateMove();
        UpdateJump();
        UpdateThrow();
    }

    private void CheckInputs()
    {
        JumpInput();
        ThrowInput();
        PickUpInput();
        MoveInput();
    }

    /* ----------------- PLAYER MOVEMENT AND JUMPING --------------------- */

    //
    private void PreviousValues()
    {
        previousFireSpeed = FireSpeed;
        previousNoFireSpeed = NoFireSpeed;
        previousHighJump = HighJumpHeight;
        previousLowJump = LowJumpHeight;
    }
    public void ChangeSpeeds(float v)
    {
        FireSpeed *= v;
        NoFireSpeed *= v;
    }
    public void ChangeJumps(float v)
    {
        LowJumpHeight *= v;
        HighJumpHeight *= v;
    }

    public void ResetValues()
    {
        FireSpeed = previousFireSpeed;
        NoFireSpeed = previousNoFireSpeed;
        HighJumpHeight = previousHighJump;
        LowJumpHeight = previousLowJump;
    }

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

    //Comprueva CADA UPDATE si se tiene que hacer el addforce para hacer el maximo salto
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
        return _collCheck.Colliding;
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
        if (_multipleJumpsLeft == MaxJumps)
        {
            if (!OnGround())
            {
                _multipleJumpsLeft--;
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
        if (Time.time - _jumpStartTime >= PressTimeToHighJump && _firstAddedForce && _multipleJumpsLeft == MaxJumps - 1)
        {
            var currentPosition = transform.position;
            //Le pasamos la altura que le queda para llegar a la HighJumpHeight
            AddJumpForce(HighJumpHeight - Vector2.Distance(currentPosition, _initialPosition));
            _firstAddedForce = false;
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
        _firstAddedForce = true;
    }
    private void ResetJumps()
    {
        _multipleJumpsLeft = MaxJumps;
    }

    /* ------------- FIRE THROWER ---------- */
   
    private void UpdateThrow() // Se llama cada update
    {
        _lr.positionCount = 0;
        if (_isChargingThrow)
        {
            //DIBUJA LA LINIA DEL LANZAMIENTO CON EL COMPONENTE LINE RENDERER (_lr)
            _lr.positionCount = ParabolicShootMaxPoints;
            List<Vector3> l_Positions = GetParabolicPositions(Fire.transform.position, (Vector2.Angle(Vector2.right, GetMouseDir())) * Mathf.Deg2Rad, 
                GetCurrentThrowSpeed(), ParabolicShootMaxPoints, ParabolicShootTime);
            _lr.SetPositions(l_Positions.ToArray());
        }
    }
    private void ThrowInput()
    {
        //Si no tiene fuego, hace return directamente
        if (!_hasFire)
            return;
        if (_isChargingThrow && Input.GetKeyDown(CancelThrowKey))
        {
            CancelThrow();
        }
        if (Input.GetKeyDown(ThrowKey))
            ThrowFireStart();
        if (Input.GetKeyUp(ThrowKey))
            ThrowFireFinish();
    }


    private void ThrowFireStart()
    {
        //empieza a cargar el disparo y se guarda el momento de inicio
        _isChargingThrow = true;
        _throwStartTime = Time.time;
    }
    private void ThrowFireFinish()
    {
        if (!_isChargingThrow)
            return;
        //Se calcula la direccion, velocidad (dependiendo del tiempo) y se lanza el fuego.
        Vector2 dir = GetMouseDir();
        float currentThrowSpeed = GetCurrentThrowSpeed();
        ThrowFire(dir, currentThrowSpeed);
    }

    private void ThrowFire(Vector2 dir, float speed)
    {
        //se llama el metodo de FireController para lanzar el fuego, dandole dir y speed.
        Fire.BeThrown(dir, speed);
        //ya no tiene fuego y no esta cargando
        _isChargingThrow = false;
    }
    private void CancelThrow()
    {
        _isChargingThrow = false;
    }

    public void SetHasFire(bool v)
    {
        _hasFire = v;
        _currentSpeed = _hasFire ? FireSpeed : NoFireSpeed;
    }

    private void PickUpInput()
    {
        if (_hasFire)
            return;
        if (Input.GetKeyDown(PickUpKey))
            TryPickUp();
    }
    
    //Try pick uf the fire
    private void TryPickUp()
    {
        if (Fire.OnPickUpRange)
            PickUpFire();
    }

    private void PickUpFire()
    {
        Fire.BePickedUp();
    }

    private Vector2 GetMouseDir()
    {
        return (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
    }
    private float GetCurrentThrowSpeed()
    {
        float timeFraction = Mathf.Clamp01((Time.time - _throwStartTime) / (TimeMaxThrow));
        float speed = Mathf.Lerp(MinThrowSpeed, MaxThrowSpeed, timeFraction);
        return speed;
    }
    List<Vector3> GetParabolicPositions(Vector2 initPos, float AngleInRadians, float Speed, int MaxPoints, float MaxTime)
    {
        List<Vector3> l_Positions = new List<Vector3>();
        float l_SpeedX = Mathf.Cos(AngleInRadians) * Speed;
        float l_SpeedY = Mathf.Sin(AngleInRadians) * Speed;
        float l_PositionY = initPos.y;
        for (int i = 0; i <= MaxPoints; ++i)
        {
            float l_Time = (i / (float)MaxPoints) * MaxTime;
            float l_DeltaTime = MaxTime / (float)MaxPoints;
            Vector3 l_Position = new Vector3(initPos.x + l_SpeedX * l_Time, l_PositionY);
            l_Positions.Add(l_Position);
            l_PositionY += l_SpeedY * l_DeltaTime;
            l_SpeedY += Physics.gravity.y * l_DeltaTime;
        }
        return l_Positions;
    }
}
