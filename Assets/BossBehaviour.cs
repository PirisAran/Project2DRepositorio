using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Transform _player;
    [SerializeField] float _xSpeedMax = 10, _ySpeedMax = 200;
    static SpeedBoostersBoss _currentBoost;

    public static void SetCurrentBoost(SpeedBoostersBoss speedBoostersBoss)
    {
        _currentBoost = speedBoostersBoss;
    }

    private void OnDrawGizmos()
    {
        if (_currentBoost == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + 10 * _currentBoost.Dir);
    }


    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    void Update()
    {
        if (_currentBoost == null) return;
        _rb.velocity = _currentBoost.BoostSpeed * _currentBoost.Dir;
        Debug.Log(_currentBoost.BoostSpeed);
    }
}
