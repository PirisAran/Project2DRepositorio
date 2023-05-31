using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TecnocampusProjectII;
public class FallingTrapActivator : MonoBehaviour
{
    Rigidbody2D _rb;

    [SerializeField] Collider2D _parentCol;

    //Collision2D _rockCollider;
    [SerializeField]
    GameObject _player;

    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.gameObject;
    }

    private void Awake()
    {
        //_rockCollider = GetComponentInParent<Collision2D>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
        {
            _parentCol.isTrigger = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
