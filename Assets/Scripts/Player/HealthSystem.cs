using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;
using System;

public class HealthSystem : MonoBehaviour
{
    public void KillPlayer()
    {
        var l_gameLogic = GameLogic.GetGameLogic();
        l_gameLogic.GetGameController().GetLevelController().RestartLevel();
    }
}
