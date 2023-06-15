using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TecnocampusProjectII;
using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera _virtualCamera;

    PlayerController _player;

    // Start is called before the first frame update
    void Start()    
    {
        _player = GameLogic.GetGameLogic().GetGameController().m_Player;
        _virtualCamera.Follow = _player.transform;
        _virtualCamera.LookAt = _player.transform;
        RoomCamManager camManager = RoomCamManager.GetCameraManager();
        camManager.AddToCamList(_virtualCamera);
        camManager.SetCurrentCam(_virtualCamera);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
