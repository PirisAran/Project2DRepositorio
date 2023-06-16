using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class BigFireActivator : PlayerWithFireActivation
{
    LevelController _currentLevelController;

    void Start()
    {
        _currentLevelController = GameLogic.GetGameLogic().GetGameController().GetLevelController();
    }

    protected override void Activate()
    {
        _currentLevelController.LoadNextScene();
    }
}
