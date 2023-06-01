using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class FireTriggerActivation : ActivationObject, IRestartLevelElement
{
    FireController _fire;
    Thrower _thrower;
    bool _activated = false;
    [SerializeField] Animator _anim;
    private void Start()
    {
        _fire = FindObjectOfType<FireController>();
        _thrower = FindObjectOfType<Thrower>();
        _anim = GetComponent<Animator>();
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_activated) return;
        if (collision.transform == _fire.transform || (collision.transform == _thrower.transform && _thrower.HasFire))
        {
            Activate();
        }

        Activate();
    }

    protected override void Activate()
    {
        OnActivated?.Invoke();
        DoAnimation();
    }

    protected override void DoAnimation()
    {
        _anim.SetBool("activated",true);
    }

    public void RestartLevel()
    {
        _activated = false;
        UndoAnimation();
    }

    private void UndoAnimation()
    {
            
        _anim.Rebind();
        _anim.Update(0f);
    }
}
