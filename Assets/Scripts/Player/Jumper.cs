using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Action OnSecondJump;

    //Components
    PlayerGroundChecker _collCheck;
    Rigidbody2D _rb;
    Thrower _thrower;

    [SerializeField] SoundPlayer _jumpingSound;
    [SerializeField] SoundPlayer _landingSound;

    //Input Jumping
    [SerializeField] KeyCode JumpKey = KeyCode.Space;

    //Jumping------------------------
    [SerializeField]
    float _highJumpHeight = 2.5f, _lowJumpHeight = 1.0f;
    public float HighJump { get { return _highJumpHeight; } set { _highJumpHeight = value; } }
    public float LowJump { get { return _lowJumpHeight; } set { _lowJumpHeight = value; } }
    float _previousHighJUmp, _previousLowJump;
    [SerializeField]
    float PressTimeToHighJump = 0.1f;
    float _jumpStartTime;
    
    //Coyote time
    [SerializeField] float _coyoteTime;
    float _timer; // controla el tiempo que esta en el aire, despues de dejar de tocar el suelo
    bool _canJump; // es la variable que se mantiene true si el jugador puede saltar. Es true cuando: esta tocando suelo, hace poco que ha dejado de tocar suelo. Es false cuando: ya ha saltado una vez o ya hace tiempo que ha dejado de tocar suelo
    bool _jumping; // es true desde que salta hasta que aterriza
    [SerializeField]
    float MaxJumps = 2;
    float _multipleJumpsLeft;
    Vector2 _initialPosition;
    public float YSpeed { get; private set; }
    bool _pressingJumpKey = false; // es true desde que pulsa la tecla, salta y hasta que la suelta.
    bool _firstAddedForce = true;
    bool _hasFire = true;

    // Start is called before the first frame update
    void Awake()
    {
        _collCheck = GetComponent<PlayerGroundChecker>();
        _rb = GetComponent<Rigidbody2D>();
        _thrower = GetComponent<Thrower>();
        GetPreviousJumps();
    }

    private void OnEnable()
    {
        _collCheck.OnLanding += OnLanding;
    }
    private void OnDisable()
    {
        _collCheck.OnLanding -= OnLanding;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCoyoteTime();
        if (!PauseMenu._isPaused)
        {
            JumpInput();
            UpdateJump();
        }
    }

    private void UpdateCoyoteTime()
    {
        _canJump = false;
        if (_collCheck.OnGround)
        {
            _timer = 0;
            _canJump = true;
        }
        else if (!_jumping)
        {
            _canJump = _timer <= _coyoteTime;
            _timer += Time.fixedDeltaTime;
        }
    }

    private void UpdateJump()
    {
        //Comprueba si tiene el fuego y si esta saltando, que son las condiciones para poder hacer el salto alto
        _hasFire = _thrower.HasFire;
        if (!_hasFire && _pressingJumpKey)
            TryAddExtraJumpForce();

        YSpeed = _rb.velocity.y;
    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(JumpKey))
            JumpStarted();
        if (Input.GetKeyUp(JumpKey))
            JumpFinished();
    }
    private bool CanJump()
    {
        return _canJump;
    }
    private void JumpStarted()
    {
        if (!_hasFire)
        {
            DoMultipleJump();
            return;
        }
        if (CanJump())
            DoJump(_lowJumpHeight);
    }
    //SALTO SIN EL FUEGO
    private void DoMultipleJump()
    {
        if (_multipleJumpsLeft == MaxJumps)
        {
            if (!CanJump())
            {
                _multipleJumpsLeft--;
            }
        }

        if (_multipleJumpsLeft > 0)
        {
            if (_multipleJumpsLeft == 1)
            {
                OnSecondJump?.Invoke();
                if (_rb.velocity.y < 0.0f)
                {
                    DoJump(_lowJumpHeight);
                }
                else
                {
                    float maxHeight = _lowJumpHeight + _highJumpHeight;
                    float distToMaxHeight = maxHeight - (transform.position.y - _initialPosition.y);
                    DoJump(distToMaxHeight);
                }
            }
            else
                DoJump(_lowJumpHeight);
            _multipleJumpsLeft--;
        }
    }
    private void DoJump(float height)
    {
        _jumping = true;
        _pressingJumpKey = true;
        _jumpStartTime = Time.time;
        AddJumpForce(height);
        _jumpingSound.PlaySound();
    }
    // SOLO SE HACE ESTO CUANDO NO TIENE EL FUEGO
    private void TryAddExtraJumpForce()
    {
        if (Time.time - _jumpStartTime >= PressTimeToHighJump && _firstAddedForce && _multipleJumpsLeft == MaxJumps - 1)
        {
            var currentPosition = transform.position;
            //Le pasamos la altura que le queda para llegar a la HighJumpHeight
            float distToMaxHeight = _highJumpHeight - (currentPosition.y - _initialPosition.y);
            AddJumpForce(distToMaxHeight);
            _firstAddedForce = false;
        }
    }
    //Añadir fuerza al jugador para que salte el jugador con altura deseada. Se llama al pulsar el SPACE // SOLO SIN EL FUEGOOO
    private void AddJumpForce(float heightToReach)
    {
        _initialPosition = transform.position;

        Debug.Log(heightToReach + " height");

        if (heightToReach < 0)
        {
            heightToReach = 0;
        }
        float jumpForce = GetJumpForce(heightToReach);

        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
    }
    //Pillar la fuerza del salto
    private float GetJumpForce(float height)
    {
        return Mathf.Sqrt(2 * Physics.gravity.magnitude * height);
    }
    private void JumpFinished()
    {
        //Se acaba el salto
        _pressingJumpKey = false;
    }
    private void OnLanding()
    {
        _landingSound.PlaySound();
        _jumping = false;
        ResetJumps();
        _firstAddedForce = true;
    }
    private void ResetJumps()
    {
        _multipleJumpsLeft = MaxJumps;
    }
    private void GetPreviousJumps()
    {
        _previousHighJUmp = _highJumpHeight;
        _previousLowJump = _lowJumpHeight;
    }
    public void SetPreviousJumps()
    {
        _highJumpHeight = _previousHighJUmp;
        _lowJumpHeight = _previousLowJump;
    }
}
