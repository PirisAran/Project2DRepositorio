using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Transform _playerSpawnPoint;
    [SerializeField] Transform _umbraSpawnPoint;
    [SerializeField] KeyCode _interactKey = KeyCode.E;

    bool _activated = false;

    SpriteRenderer _spriteRenderer;

    public static Action OnCheckPointActivated;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Thrower thrower = collision.GetComponent<Thrower>();
        if (thrower == null)
            return;

        if (_activated)
            return;
        
        if (Input.GetKeyDown(_interactKey) && thrower.HasFire)
        {
            ActivateCheckpoint();
            ActivateAnim();
        }
    }

    private void ActivateAnim()
    {
        _spriteRenderer.color = Color.green;
    }

    private void ActivateCheckpoint()
    {
        Debug.Log("checkpoint activated");
        OnCheckPointActivated?.Invoke();
        LevelController.Instance.SetSpawnpoint(_playerSpawnPoint.position, _umbraSpawnPoint.position);
        _activated = true;

    }
}
