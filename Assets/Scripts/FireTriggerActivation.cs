using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerActivation : ActivationObject
{
    FireController _fire;
    bool _activated = false;
    [SerializeField] Animator _animator;
    private void Start()
    {
        _fire = FindObjectOfType<FireController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != _fire.transform) return;
        if (_activated) return;

        Activate();
    }

    protected override void Activate()
    {
        OnActivated?.Invoke();
        DoAnimation();
    }

    protected override void DoAnimation()
    {
        _animator.SetBool("activated",true);
    }

}
