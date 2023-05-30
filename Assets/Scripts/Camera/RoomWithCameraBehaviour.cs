using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWithCameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    public CinemachineVirtualCamera Cam => _virtualCamera;
    private bool _playerInTrigger = false;
    public bool PlayerInTrigger => _playerInTrigger;
    RoomCamManager _roomCamManager;
    
    [SerializeField]
    float _scale;

    private void OnValidate()
    {
        UpdateCamSize();
    }

    private void UpdateCamSize()
    {
        transform.localScale = new Vector2(16, 9) * _scale;
        _virtualCamera.m_Lens.OrthographicSize = transform.localScale.y / 2;
    }

    private void Awake()
    {
        _virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        _roomCamManager =  RoomCamManager.GetCameraManager();
        _roomCamManager.AddToRoomList(this);
    }

    private void Start()
    {
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = true;
            _roomCamManager.SetCurrentRoomCam(this);
        }

    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInTrigger = false;
        }
    }
}
