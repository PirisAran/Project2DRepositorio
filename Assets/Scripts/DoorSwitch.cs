using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorSwitch : FireActivationObject
{
    SpriteRenderer _spriteRenderer;

    public Action OnSwitchActivated;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void DoAnimation()
    {
        _spriteRenderer.color = Color.red + Color.yellow;
    }

    protected override void Activate()
    {
        base.Activate();
        OnSwitchActivated?.Invoke();
    }
}
