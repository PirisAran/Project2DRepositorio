using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FireDamager : MonoBehaviour
{
    [SerializeField] bool _canDamage;
    [SerializeField] float _damageDealt;
    public float Damage => _damageDealt;

    static FireController _fire;
    static PlayerController _player;

    private void Start()
    {
        if (_player == null)
        {
            _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        }
        if (_fire == null)
        {
            _fire = _player.GetComponentInChildren<FireController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canDamage) return;

        if (collision.transform == _fire.transform)
        {
            if (_fire.IsAttached()) return;
        }
        else if (collision.transform == _player.transform)
        {
            if (!_fire.IsAttached()) return;
        }
        else return;

        OnDamageFire();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_canDamage) return;


        if (collision.gameObject.transform == _fire.transform)
        {
            if (_fire.IsAttached()) return;
        }
        else if (collision.gameObject.transform == _player.transform)
        {
            if (!_fire.IsAttached()) return;
        }
        else return;

        OnDamageFire();
    }

    
    // Deal damage to fire
    private void OnDamageFire()
    {
        _fire.TakeDamage(_damageDealt);
        Debug.Log(name + " damage fire: " + _damageDealt);
    }

    public void SetCanDamage(bool v)
    {
        _canDamage = v;
    }

}
