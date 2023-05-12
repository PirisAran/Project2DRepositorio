using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Traps : MonoBehaviour
{
    Rigidbody2D _rb;

    //Collision2D _rockCollider;

    [SerializeField]
    GameObject Player;

    private void Awake()
    {
        //_rockCollider = GetComponentInParent<Collision2D>();
        _rb = GetComponentInParent<Rigidbody2D>();
       _rb.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player.transform)
        {
            Debug.Log("detected");
            _rb.isKinematic = false;
        }
    }
}
