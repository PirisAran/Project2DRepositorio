using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public abstract class FireActivationObject : MonoBehaviour
{
    [SerializeField] protected KeyCode _interactKey = KeyCode.E;
    bool _isActivated;
    bool _canActivate = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //DETECTAR SI EL QUE ENTRA ES PLAYER, TIENE FUEGO

        if (_isActivated) return;

        Thrower thrower = collision.GetComponent<Thrower>();
        if (thrower == null) return;
        _canActivate = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _canActivate = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isActivated)
            return;
        Thrower thrower = collision.GetComponent<Thrower>();
        if (thrower != null)
        {
            if (thrower.HasFire && Input.GetKeyDown(_interactKey))
            {
                Activate();
            }
        }
    }


    private void Update()
    {
        if (_isActivated)
            return;
        Thrower thrower = collision.GetComponent<Thrower>();
        if (thrower != null)
        {
            if (thrower.HasFire && Input.GetKeyDown(_interactKey))
            {
                Activate();
            }
        }
    }
    protected virtual void Activate()
    {
        DoAnimation();
        _isActivated = true;
    }

    protected abstract void DoAnimation();
}
