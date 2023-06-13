using System;
using UnityEngine;
using TecnocampusProjectII;

public class UmbraMini : MonoBehaviour, IRestartLevelElement
{
    PlayerController _player;
    float _speed;
    Vector2 _oPosition;
    private void Awake()
    {
        _oPosition = transform.position;
        gameObject.SetActive(false);
    }
    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    public void Activate(float speed)
    {
        gameObject.SetActive(true);
        _speed = speed;
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
        
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                _player.transform.position,
                                                _speed * Time.fixedDeltaTime);

    }

    public void RestartLevel()
    {
        gameObject.SetActive(false);
        transform.position = _oPosition;
    }
}
