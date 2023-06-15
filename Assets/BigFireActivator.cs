using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BigFireActivator : PlayerWithFireActivation
{
    PlayerController _player;
    Thrower _thrower;
    LevelController _currentLevelController;

    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _thrower = _player.GetComponent<Thrower>();
        _currentLevelController = GameLogic.GetGameLogic().GetGameController().GetLevelController();
    }

    protected override void Activate()
    {
        _currentLevelController.LoadNextScene();
    }
}
