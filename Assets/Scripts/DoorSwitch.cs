using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoorSwitch : FireActivationObject
{
    SpriteRenderer _spriteRenderer;
    Animator _anim;

    public Action OnSwitchActivated;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    protected override void DoAnimation()
    {
        _anim.SetBool("activated", true);
        //StartCoroutine(DoActivationAnim());
    }

    protected override void Activate()
    {
        base.Activate();
        StartCoroutine(DoInvokeDelayed());
    }

    protected override void ShowPlayerHeCant()
    {
        StartCoroutine(DoErrorAnimation());
    }

    IEnumerator DoInvokeDelayed()
    {
        yield return new WaitForSeconds(1);
        OnSwitchActivated?.Invoke();

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
