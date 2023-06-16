using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public abstract class PlayerWithFireActivation : ActivationObject
{
    [SerializeField] protected KeyCode _interactKey = KeyCode.E;
    protected bool _isActivated = false;
    protected bool _inTrigger = false;
    Thrower _thrower;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _thrower = GameLogic.GetGameLogic().GetGameController().m_Player.GetComponent<Thrower>();
        //DETECTAR SI EL QUE ENTRA ES PLAYER, TIENE FUEGO
        if (_isActivated) return;
       
        if (collision.transform != _thrower.transform) return;
        _inTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _thrower = GameLogic.GetGameLogic().GetGameController().m_Player.GetComponent<Thrower>();
        if (_isActivated) return;

        if (_thrower.transform != collision.transform) return;
        _inTrigger = false;
    }

    private void Update()
    {
        if (_isActivated)
            return;
        if (_inTrigger && Input.GetKeyDown(_interactKey))
        {
            if (_thrower.HasFire)
            {
                Activate();
            }
            else
            {
                //ShowPlayerHeCant();
            }
        }
        
    }

    protected virtual void ShowPlayerHeCant()
    {

    }

    protected override void Activate()
    {
        DoAnimation();
        _isActivated = true;
    }
}
