using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TecnocampusProjectII;
public class FallingTrapActivator : MonoBehaviour
{
    Rigidbody2D _parentRb;

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
        _parentRb = GetComponentInParent<Rigidbody2D>();
        _parentRb.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
        {
            _parentRb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
