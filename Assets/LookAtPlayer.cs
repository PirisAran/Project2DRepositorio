using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField]
    Transform _player;

    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            Vector2 direction = _player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = targetRotation;
        }
    }
}
