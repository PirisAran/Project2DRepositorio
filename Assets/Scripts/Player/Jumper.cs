using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    //Components
    CollisionChecker _collCheck;
    Rigidbody2D _rb;
    Thrower _thrower;



    //Input Jumping
    [SerializeField] KeyCode JumpKey = KeyCode.Space;

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
    public float YSpeed { get; private set; }
    bool _doingJump = false;
    bool _firstAddedForce = true;
    bool _hasFire = true;

    // Start is called before the first frame update
    void Awake()
    {
        _collCheck = GetComponent<CollisionChecker>();
        _rb = GetComponent<Rigidbody2D>();
        _thrower = GetComponent<Thrower>();
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
        JumpInput();
        UpdateJump();
    }

    private void UpdateJump()
    {
        //Comprueba si tiene el fuego y si esta saltando, que son las condiciones para poder hacer el salto alto
        _hasFire = _thrower.HasFire;
        if (!_hasFire && _doingJump)
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
            DoJump();
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
            DoJump();
        }
    }
    private void DoJump()
    {
        _doingJump = true;
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
    //A�adir fuerza al jugador para que salte el jugador con altura deseada. Se llama al pulsar el SPACE // SOLO SIN EL FUEGOOO
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
        _doingJump = false;
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
}