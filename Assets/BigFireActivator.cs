using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BigFireActivator : MonoBehaviour
{
    [SerializeField] KeyCode _interactKey = KeyCode.E;
    PlayerController _player;
    Thrower _thrower;
    bool _inTrigger;
    LevelController _currentLevelController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != _player.transform)
        {
            return;
        }
        _inTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != _player.transform)
        {
            return;
        }
        _inTrigger = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _thrower = _player.GetComponent<Thrower>();
        _currentLevelController = GameLogic.GetGameLogic().GetGameController().GetLevelController();
    }



    // Update is called once per frame
    void Update()
    {
        if (!_inTrigger)
        {
            return;
        }

        if (_thrower.HasFire && Input.GetKeyDown(_interactKey))
        {
            _currentLevelController.LoadNextScene();
        }
    }
}
