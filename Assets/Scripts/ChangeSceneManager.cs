using System;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    [SerializeField] Transform _player;

    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == _player.transform)
        {
            StartCoroutine(ChangeScene(1));
        }
    }

    IEnumerator ChangeScene(int _nextScene)
    {
        int _currentScene = SceneManager.GetActiveScene().buildIndex;

        _currentScene += _nextScene;

        //make transition scene 

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(_currentScene);
    }
}
