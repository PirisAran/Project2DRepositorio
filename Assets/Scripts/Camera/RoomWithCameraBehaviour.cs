using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWithCameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;

    CameraManager _camManager;

    private void Awake()
    {
        //_virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        _camManager =  CameraManager.GetCameraManager();
        _camManager.AddToCameraList(_virtualCamera);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _camManager.SetCurrentCamera(_virtualCamera);
        }

    }
}
