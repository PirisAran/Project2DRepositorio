using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FireDamager : MonoBehaviour
{
    [SerializeField] bool _canDamage;
    [SerializeField] float _damageDealt;
    public float Damage => _damageDealt;

    bool _playerInTrigger;

    FireController _fire;
    PlayerController _player;


    private void OnEnable()
    {
        if (_fire==null)
        {
            return;
        }
        _fire.OnFirePickedUp += OnFirePickedUp;
    }

    private void OnDisable()
    {
        if (_fire == null)
        {
            return;
        }
        _fire.OnFirePickedUp -= OnFirePickedUp;
    }

    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _fire = FindObjectOfType<FireController>();
        _fire.OnFirePickedUp += OnFirePickedUp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canDamage) return;

        if (_fire != null && _player!= null)
        {
            if (collision.transform == _fire.transform)
            {
                if (_fire.IsAttached()) return;
            }
            else if (collision.transform == _player.transform)
            {
                _playerInTrigger = true;
                if (!_fire.IsAttached()) return;
            }
            else return;
            OnDamageFire();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
        {
            _playerInTrigger = false;
        }
    }

    private void OnFirePickedUp()
    {
        if (_playerInTrigger)
        {
            OnDamageFire();
        }
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
        if (_fire.CurrentFireHealth > 0)
        {
            _fire.TakeDamage(_damageDealt);
        }
        Debug.Log(name + " damage fire: " + _damageDealt);
    }

    public void SetCanDamage(bool v)
    {
        _canDamage = v;
    }

}
