using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TecnocampusProjectII;
public class KillPlayerOnTouch : MonoBehaviour
{
    [SerializeField]
    GameObject _player;

    private void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.gameObject;
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
        if (collision.transform == _player.transform)
        {
            _player.GetComponent<HealthSystem>().KillPlayer();
            Debug.Log("Player killed by stalactita");
        }
        Destroy(gameObject);
    }
}
