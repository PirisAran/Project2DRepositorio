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
    }

    // Start is called before the first frame update
    void Start()
    {
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

    //private bool PlayerIsInsideCollider()
    //{

    //}

    private bool FireIsInsideCollider()
    {
        return _fireInsideCollider;
    }
}
