using System;
using UnityEngine;
using TecnocampusProjectII;

public class UmbraMini : MonoBehaviour, IRestartLevelElement
{
    PlayerController _player;
    float _speed;
    Vector2 _oLocalPosition;
    Transform _parent;
    private void Awake()
    {
        _oLocalPosition = transform.localPosition;
        _parent = transform.parent;
    }
    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
        gameObject.SetActive(false);
    }

    public void Activate(float speed)
    {
        gameObject.SetActive(true);
        transform.parent = null;
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
        Debug.Log("RS MINUMBRA");
        gameObject.SetActive(false);
        transform.parent = _parent;
        transform.localPosition = _oLocalPosition;
    }
}
