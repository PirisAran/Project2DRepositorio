using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FinishLevelColliderBehaviour : MonoBehaviour
{
    PlayerController _player;
    FireController _fire;

    private bool _fireInsideCollider;
    private bool _playerInsideCollider;

    public bool CanFinishLevel { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _fire.transform)
        {
            _fireInsideCollider = true;
        }

        if (collision.transform == _player.transform)
        {
            _playerInsideCollider = true;
            if (_fire.IsAttached())
            {
                _fireInsideCollider = true;
            }
        }

        Debug.Log(_fireInsideCollider + " fire enter");
        Debug.Log(_playerInsideCollider + " player enter");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == _fire.transform)
        {
            _fireInsideCollider = false;
        }

        if (collision.transform == _player.transform)
        {
            _playerInsideCollider = false;
            if (_fire.IsAttached())
            {
                _fireInsideCollider = false;
            }
        }

        Debug.Log(_fireInsideCollider + " fire out");
        Debug.Log(_playerInsideCollider + " player out");
    }

    // Start is called before the first frame update
    void Start()
    {
        CanFinishLevel = false;
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _fire = _player.GetComponentInChildren<FireController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCanFinishLevel();
    }

    private void UpdateCanFinishLevel()
    {
        CanFinishLevel = PlayerIsInsideCollider() && FireIsInsideCollider();
    }

    private bool PlayerIsInsideCollider()
    {
        return _playerInsideCollider;
    }

    private bool FireIsInsideCollider()
    {
        return _fireInsideCollider;
    }
}
