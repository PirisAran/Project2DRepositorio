using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FallingBlockTrap : MonoBehaviour, IRestartLevelElement
{
    [SerializeField]
    GameObject _player;

    [SerializeField]
    bool _canReset = true;
    Rigidbody2D _rb;
    Collider2D _collider;

    Vector2 _oPosition;
    Vector2 _previousSpeed;

    public void RestartLevel()
    {
        if (!_canReset) return;

        transform.position = _oPosition;
        _rb.bodyType = RigidbodyType2D.Static;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Start()
    {
        var gameController = GameLogic.GetGameLogic().GetGameController();
        _player = gameController.m_Player.gameObject;
        var lvlController = gameController.GetLevelController();
        lvlController.AddRestartLevelElement(this);

        _oPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //if (!_collider.IsTouchingLayers())
        //{
        //    _previousSpeed = _rb.velocity;
        //}
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_rb.bodyType == RigidbodyType2D.Static)
            return;

        if (collision.gameObject.GetComponent<FireController>())
        {

            if (collision.rigidbody.velocity.magnitude != 0)
            {
                //_rb.velocity = _previousSpeed;
                return;
            }
            float xOffset = collision.transform.position.x - transform.position.x;
            float width = GetComponent<BoxCollider2D>().bounds.size.x;

            float closestEdgeDist = width / 2 - Mathf.Abs(xOffset);
            float closestEdgeDir = Mathf.Sign(xOffset);

            Debug.Log("before translation");
            collision.transform.Translate(new Vector2(closestEdgeDir * (closestEdgeDist + collision.collider.bounds.size.x / 2), 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_rb.bodyType == RigidbodyType2D.Static)
            return;

        if (collision.transform == _player.transform)
        {
            collision.gameObject.GetComponent<HealthSystem>().KillPlayer();
        }
    }
}
