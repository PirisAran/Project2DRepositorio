using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
