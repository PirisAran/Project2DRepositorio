using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TecnocampusProjectII;

public class DoorButton : PlayerWithFireActivation, IRestartLevelElement
{
    SpriteRenderer _spriteRenderer;
    Animator _anim;

    private void Start()
    {
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    protected override void DoAnimation()
    {
        _anim.SetBool("activated", true);
    }

    protected override void Activate()
    {
        base.Activate();
        StartCoroutine(DoInvokeDelayed());
    }


    IEnumerator DoInvokeDelayed()
    {
        yield return new WaitForSeconds(1);
        OnActivated?.Invoke();

    }

    public void RestartLevel()
    {
        _isActivated = false;
        UndoAnimation();
    }
    private void UndoAnimation()
    {
        _anim.Rebind();
        _anim.Update(0f);
    }
}
