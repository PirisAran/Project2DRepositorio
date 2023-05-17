using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class FallinPlatform : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] FireController _fire;
    [SerializeField] Transform _oPosition, _fPosition;
    [SerializeField] Collider2D _detectionCollider;
    Thrower _playerThrower;
    Collider2D _playerCollider;
    Collider2D _fireCollider;
    States _currentState;

    [Header ("Movement Speeds")]
    [SerializeField] float _upwardsSpeed;
    [SerializeField] float _playerMassSpeed;
    [SerializeField] float _fireMassSpeed;
    
    private enum States {Idle, MovingDown, MovingUp}
    private void Awake()
    {
        _currentState = States.Idle;
    }
    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
        _playerThrower = _player.GetComponent<Thrower>();
        _playerCollider = _player.GetComponentInChildren<BoxCollider2D>();

        _fire = _player.GetComponentInChildren<FireController>();
        _fireCollider = _fire.GetComponent<Collider2D>();
    }

    private void ChangeState(States nextState)
    {
        _currentState = nextState;
        Debug.Log("Entered " + nextState);
    }

    private void FixedUpdate()
    {
        Debug.Log(FireOnPlatform());
        switch (_currentState)
        {
            case States.Idle:
                UpdateIdleState();
                break;
            case States.MovingDown:
                UpdateMovingDownState();
                break;
            case States.MovingUp:
                UpdateMovingUpState();
                break;
            default:
                break;
        }
    }

    private void UpdateIdleState()
    {
        if (PlayerOnPlatform())
        {
            ChangeState(States.MovingDown);
            return;
        }
    }


    private void UpdateMovingDownState()
    {
        if (!PlayerOnPlatform() && !FireOnPlatform() && transform.position != _oPosition.position)
        {
            ChangeState(States.MovingUp);
            return;
        }

        float downSpeed;

        if (PlayerOnPlatform() && !PlayerHasFire() && !FireOnPlatform())
        {
            downSpeed = _playerMassSpeed;
        }
        else if (FireOnPlatform() && !PlayerOnPlatform())
        {
            downSpeed = _fireMassSpeed;
        }
        else
        {
            downSpeed = _playerMassSpeed + _fireMassSpeed;
        }

        MovePlatform(downSpeed, _fPosition.position);
    }

    private void MovePlatform(float downSpeed, Vector2 desiredPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, desiredPosition, downSpeed * Time.fixedDeltaTime);
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

        MovePlatform(_upwardsSpeed, _oPosition.position);
    }


}
