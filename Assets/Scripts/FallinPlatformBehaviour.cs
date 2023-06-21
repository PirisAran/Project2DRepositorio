using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class FallinPlatformBehaviour : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] FireController _fire;
    [SerializeField] Transform _oPosition, _fPosition;
    [SerializeField] Collider2D _detectionCollider;
    [SerializeField] SoundPlayer _fallingSound;
    Thrower _playerThrower;
    Collider2D _playerCollider;
    Collider2D _fireCollider;
    States _currentState;

    [Header ("Movement Speeds")]
    [SerializeField] float _upwardsSpeed;
    [SerializeField] float _playerMassSpeed;
    [SerializeField] float _fireMassSpeed;

    private bool _isMoving = false;
    private AudioSource _audioSource;
    float _oVolume;

    private enum States {Idle, MovingDown, MovingUp}
    private void Awake()
    {
        _currentState = States.Idle;

    }
    private void Start()
    {
        _audioSource = _fallingSound.PlaySound().GetComponent<AudioSource>();
        _oVolume = _audioSource.volume;
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        _playerThrower = _player.GetComponent<Thrower>();
        _playerCollider = _player.GetComponentInChildren<PolygonCollider2D>();

        _fire = _player.GetComponentInChildren<FireController>();
        _fireCollider = _fire.GetComponent<Collider2D>();
    }

    private void ChangeState(States nextState)
    {
        _currentState = nextState;
        Debug.Log("Entered " + nextState);
        _isMoving = _currentState != States.Idle;
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case States.MovingDown:
                UpdateMovingDownState();
                break;
            case States.MovingUp:
                UpdateMovingUpState();
                break;
            case States.Idle:
                UpdateIdleState();
                break;
            default:
                break;
        }
        UpdateSound();
    }

    private void UpdateSound()
    {
        _audioSource.volume = _isMoving ? _oVolume : 0;
        Debug.Log(_audioSource.volume + " VOLUME");
    }

    private void UpdateIdleState()
    {
        if (PlayerOnPlatform() || FireOnPlatform())
        {
            ChangeState(States.MovingDown);
        }
    }

    private void UpdateMovingDownState()
    {
        _isMoving = transform.position != _fPosition.position;

        if (!PlayerOnPlatform() && !FireOnPlatform() && transform.position != _oPosition.position)
        {
            ChangeState(States.MovingUp);
            return;
        }

        float speed;

        if (PlayerOnPlatform() && !PlayerHasFire() && !FireOnPlatform())
        {
            speed = _playerMassSpeed;
            MoveAlongPlatform(speed, _fPosition.position, _player);
        }
        else if (FireOnPlatform() && !PlayerOnPlatform())
        {
            speed = _fireMassSpeed;
            MoveAlongPlatform(speed, _fPosition.position, _fire.transform);
        }
        else
        {
            speed = _playerMassSpeed + _fireMassSpeed;
            MoveAlongPlatform(speed, _fPosition.position, _player);
            MoveAlongPlatform(speed, _fPosition.position, _fire.transform);
        }
        MovePlatform(speed, _fPosition.position);
    }

    private void MovePlatform(float speed, Vector2 desiredPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, speed * Time.fixedDeltaTime);
    }
    private void MoveAlongPlatform(float speed, Vector2 platformDesiredPosition, Transform entity)
    {
        Vector2 entityPlatformOffset = entity.position - transform.position;
        Vector2 entityDesiredPos = platformDesiredPosition + entityPlatformOffset;
        entity.position = Vector2.MoveTowards(entity.position, entityDesiredPos, speed * Time.fixedDeltaTime);
    }

    private bool PlayerHasFire()
    {
        return _playerThrower.HasFire;
    }

    //Comprueva si el fuego esta tocando la plataforma
    private bool FireOnPlatform()
    {
        return _fireCollider.IsTouching(_detectionCollider);
    }

    //Comprueva si el player esta tocando la paltaforma
    private bool PlayerOnPlatform()
    {
        return _detectionCollider.IsTouching(_playerCollider);
    }

    private void UpdateMovingUpState()
    {
        if (PlayerOnPlatform() || FireOnPlatform())
        {
            ChangeState(States.MovingDown);
        }
        else if (transform.position == _oPosition.position)
        {
            ChangeState(States.Idle);
            _isMoving = false;
        }

        MovePlatform(_upwardsSpeed, _oPosition.position);
    }




}
