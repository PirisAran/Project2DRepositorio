using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class FallingBlockTrap : MonoBehaviour, IRestartLevelElement
{
    [SerializeField]
    GameObject _player;

    Rigidbody2D _rb;

    Vector2 _oPosition;

    public void RestartLevel()
    {
        transform.position = _oPosition;
        _rb.bodyType = RigidbodyType2D.Static;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Start()
    {
        var gameController = GameLogic.GetGameLogic().GetGameController();
        _player = gameController.m_Player.gameObject;
        gameController.GetLevelController().AddRestartLevelElement(this);
        _oPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_rb.bodyType == RigidbodyType2D.Static) return;

        if (collision.transform == _player.transform)
        {
            if (_rb.bodyType != RigidbodyType2D.Static)
            {
                _player.GetComponent<HealthSystem>().KillPlayer();
            }
        }
        else if (collision.gameObject.GetComponent<FireController>())
        {
            if (collision.rigidbody.velocity.magnitude != 0)
            {
                return;
            }
            float xOffset = collision.transform.position.x - transform.position.x;
            float width = GetComponent<BoxCollider2D>().bounds.size.x;

            float closestEdgeDist = width / 2 - Mathf.Abs(xOffset);
            float closestEdgeDir = Mathf.Sign(xOffset);

            Debug.Log("before translation");
            collision.transform.Translate(new Vector2(closestEdgeDir * (closestEdgeDist + collision.collider.bounds.size.x / 2), 0));
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Static;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
