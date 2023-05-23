using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TecnocampusProjectII;

public class HealthSystem : MonoBehaviour
{
    public void KillPlayer()
    {
        LevelController.Instance.RespawnPlayer();
        FindObjectOfType<FireController>().OnPlayerRespawn();
    }
}
