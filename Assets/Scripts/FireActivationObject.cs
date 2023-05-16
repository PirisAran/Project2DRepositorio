using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public abstract class FireActivationObject : MonoBehaviour
{
    [SerializeField] protected KeyCode _interactKey = KeyCode.E;
    protected bool _isActivated;
    protected bool _canActivate = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //DETECTAR SI EL QUE ENTRA ES PLAYER, TIENE FUEGO
        if (_isActivated) return;

        Thrower thrower = collision.GetComponent<Thrower>();
        if (thrower == null) return;

        if (thrower.HasFire)
        {
            _canActivate = true;
            Debug.Log("Player is inside");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_isActivated) return;

        Thrower thrower = collision.GetComponent<Thrower>();
        if (thrower == null) return;

        if (thrower.HasFire)
        {
            Debug.Log("Player is outside");
            _canActivate = false;
        }
    }

    private void Update()
    {
        if (_isActivated)
            return;

        if (_canActivate && Input.GetKeyDown(_interactKey))
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
