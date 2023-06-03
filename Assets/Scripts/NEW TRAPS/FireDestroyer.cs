using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FireDestroyer : MonoBehaviour
{
    [SerializeField] bool _canDestroy;

    FireController _fire;
    // Start is called before the first frame update
    void Start()
    {
        _fire = GameLogic.GetGameLogic().GetGameController().m_Player.GetComponentInChildren<FireController>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canDestroy) return;

        if (collision.transform == _fire.transform)
        {
            if (_fire.IsAttached()) return;
            OnDestroyFire();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_canDestroy) return;

        if (collision.gameObject.transform == _fire.transform)
        {
            if (_fire.IsAttached()) return;
            OnDestroyFire();
        }
    }

    private void OnDestroyFire()
    {
        _fire.DestroyFire();
    }

    internal void SetCanDestroy(bool v)
    {
        _canDestroy = v;
    }
}
