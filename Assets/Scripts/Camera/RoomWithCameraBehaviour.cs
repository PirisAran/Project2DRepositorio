using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWithCameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _virtualCamera;
    [SerializeField]
    BoxCollider2D _boxCollider;
    
    [Range (0f, 1f)]
    [SerializeField]
    float _colliderFractionSize = 0.8f;
    public CinemachineVirtualCamera Cam => _virtualCamera;
    private bool _playerInTrigger = false;
    public bool PlayerInTrigger => _playerInTrigger;
    RoomCamManager _roomCamManager;
    
    [SerializeField]
    float _scale;
    [SerializeField]
    private bool _freeForm = false;

    private void OnValidate()
    {
        UpdateBoxSize();
        UpdateCamSize();
        if (_freeForm) return;    
        UpdateBoxCollider();
    }

    private void UpdateBoxCollider()
    {
        _boxCollider.size = Vector2.one * _colliderFractionSize;
    }

    private void UpdateBoxSize()
    {
        transform.localScale = new Vector2(16, 9) * _scale;
    }

    private void UpdateCamSize()
    {
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

    private void OnDrawGizmos()
    {
        if (_playerInTrigger)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
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
