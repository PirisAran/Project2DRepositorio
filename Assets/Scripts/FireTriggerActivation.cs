using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerActivation : ActivationObject
{
    FireController _fire;
    Thrower _thrower;
    bool _activated = false;
    [SerializeField] Animator _animator;
    private void Start()
    {
        _fire = FindObjectOfType<FireController>();
        _thrower = FindObjectOfType<Thrower>();

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
        _animator.SetBool("activated",true);
    }

}
