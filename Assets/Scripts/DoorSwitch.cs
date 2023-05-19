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

    protected override void ShowPlayerHeCant()
    {
        StartCoroutine(DoErrorAnimation());
    }
    
    IEnumerator DoErrorAnimation()
    {
        Color previousColor = _spriteRenderer.color;
        
        for (int i = 0; i < 3; i++)
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            _spriteRenderer.color = previousColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
