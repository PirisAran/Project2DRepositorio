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


    [SerializeField] Animator _eIconAnim;

    [SerializeField] SoundPlayer _errorSound;

    private enum IconStates { Show, Hide, PressedGood, PressedBad}
    IconStates _currentState;
    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_thrower == null)
        {
            _thrower = GameLogic.GetGameLogic().GetGameController().m_Player.GetComponent<Thrower>();
        }
        //DETECTAR SI EL QUE ENTRA ES PLAYER, TIENE FUEGO
        if (_isActivated) return;
       
        if (collision.transform != _thrower.transform) return;
        _inTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_thrower == null)
        {
            _thrower = GameLogic.GetGameLogic().GetGameController().m_Player.GetComponent<Thrower>();
        }
        if (_isActivated) return;

        if (_thrower.transform != collision.transform) return;
        _inTrigger = false;
    }

    private void Update()
    {
        if (_isActivated)
        {
            if (_eIconAnim != null)
            {
                if (_currentState != IconStates.Hide)
                {
                    ChangeAnimIcon(IconStates.Hide);
                    _eIconAnim.SetBool("activated", true);
                }
            }
            return;
        }
        UpdateEIconAnimation();
        if (_inTrigger && Input.GetKeyDown(_interactKey))
        {
            if (_thrower.HasFire)
            {
                Activate();
            }
            else
            {
                if (_eIconAnim != null)
                {
                    ChangeAnimIcon(IconStates.PressedBad);
                    _errorSound.PlaySound();
                }
            }
        }
        
    }
    

    private void UpdateEIconAnimation()
    {
        if (_eIconAnim == null)
            return;
        IconStates state = IconStates.Hide;
        
        if (_inTrigger)
        {
            state = IconStates.Show;
        }

        ChangeAnimIcon(state);
    }

    private void ChangeAnimIcon(IconStates state)
    {
        if (_currentState == state)
        {
            return;
        }
        _eIconAnim.SetInteger("state", (int)state);
        _currentState = state;
    }

    protected override void Activate()
    {
        DoAnimation();
        _isActivated = true;
    }

}
