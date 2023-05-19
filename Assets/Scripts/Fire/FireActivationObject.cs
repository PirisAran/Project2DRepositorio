using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public abstract class FireActivationObject : MonoBehaviour
{
    [SerializeField] protected KeyCode _interactKey = KeyCode.E;
    protected bool _isActivated = false;
    protected bool _canActivate = false;
    Thrower _thrower;

    private void Start()
    {
        _thrower = FindObjectOfType<Thrower>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //DETECTAR SI EL QUE ENTRA ES PLAYER, TIENE FUEGO
        if (_isActivated) return;

        
        if (_thrower.transform != collision.transform) return;
        _canActivate = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isActivated) return;

        if (_thrower.transform != collision.transform) return;
        _canActivate = false;
    }

    private void Update()
    {
        if (_isActivated)
            return;

        if (_canActivate && Input.GetKeyDown(_interactKey) && _thrower.HasFire)
        {
            Activate();
        }
    }
    protected virtual void Activate()
    {
        DoAnimation();
        _isActivated = true;
    }

    protected abstract void DoAnimation();
}
