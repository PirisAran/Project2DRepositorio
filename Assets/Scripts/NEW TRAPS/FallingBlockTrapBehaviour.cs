using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FallingBlockTrapBehaviour : MonoBehaviour, IRestartLevelElement
{
    [Header("Scripts Utilizados")]
    [SerializeField] PlayerKiller _playerKiller;
    [SerializeField] PlayerDetector _playerDetector;
    [SerializeField] FireDestroyer _fireDestroyer;
    [SerializeField] SoundPlayer _fallingSound;
    [SerializeField] SoundPlayer _impactSound;

    [Space]

    [SerializeField]
    Collider2D _landingSurface;
    [SerializeField]
    bool _canReset = true;

    PlayerController _player;
    FireController _fire;
    Collider2D _collider;
    Rigidbody2D _rb;
    Vector2 _oPosition;
    Vector2 _previousSpeed;

    private void OnEnable()
    {
        Debug.Log(_playerDetector == null);
        _playerDetector.OnPlayerDetected += OnPlayerDetected;
    }

    private void OnDisable()
    {
        _playerDetector.OnPlayerDetected -= OnPlayerDetected;
    }

    public void RestartLevel()
    {
        if (!_canReset) return;

        transform.position = _oPosition;
        _rb.bodyType = RigidbodyType2D.Static;
        transform.GetChild(0).gameObject.SetActive(true);
        _fireDestroyer.SetCanDestroy(false);
        _playerKiller.SetCanKill(false);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        Init();
    }

    private void Init()
    {
        _oPosition = transform.position;
        _playerKiller.SetCanKill(false);
        _fireDestroyer.SetCanDestroy(false);
    }


    private void Start()
    {
        var gameController = GameLogic.GetGameLogic().GetGameController();
        _player = gameController.m_Player;
        var lvlController = gameController.GetLevelController();
        lvlController.AddRestartLevelElement(this);
        _fire = _player.GetComponentInChildren<FireController>();
    }
    private void OnPlayerDetected()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _playerDetector.enabled = false;
        _playerKiller.SetCanKill(true);
        _fireDestroyer.SetCanDestroy(true);
        _fallingSound.PlaySound();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == _landingSurface)
        {
            OnLanded();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == _landingSurface)
        {
            OnLanded();
        }
    }

    private void OnLanded()
    {
        _playerKiller.SetCanKill(false);
        _fireDestroyer.SetCanDestroy(false);
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _impactSound.PlaySound();
    }

}
