using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TecnocampusProjectII;
public class KillPlayerOnTouch : MonoBehaviour, IRestartLevelElement
{
    [SerializeField]
    GameObject _player;

    Vector2 _oPosition;

    private void Awake()
    {
        _oPosition = transform.position;
    }
    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.gameObject;
        GameLogic.GetGameLogic().GetGameController().GetLevelController().AddRestartLevelElement(this);
    }

    

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Detected: " + collision.gameObject.name);
    //    if (collision.transform == _player.transform)
    //    {
    //        _player.GetComponent<HealthSystem>().KillPlayer();
    //    }
    //    Destroy(gameObject);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Detected: " + collision.gameObject.name);
        gameObject.SetActive(false);
        if (collision.transform == _player.transform)
        {
            _player.GetComponent<HealthSystem>().KillPlayer();
            Debug.Log("Player killed by stalactita");
        }
    }

    public void RestartLevel()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        transform.position = _oPosition;
        rb.velocity = Vector2.zero;
        gameObject.SetActive(true);
    }
}
