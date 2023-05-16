using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class CheckPoint : FireActivationObject
{
    [SerializeField] Transform _playerSpawnPoint;
    [SerializeField] Transform _umbraSpawnPoint;

    SpriteRenderer _spriteRenderer;
    public static Action OnCheckPointActivated;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void DoAnimation()
    {
        _spriteRenderer.color = Color.green;
    }

    protected override void Activate()
    {
        base.Activate();
        OnCheckPointActivated?.Invoke();
        LevelController.Instance.SetSpawnpoint(_playerSpawnPoint.position, _umbraSpawnPoint.position);
        Debug.Log("checkpoint activated");
    }
}
