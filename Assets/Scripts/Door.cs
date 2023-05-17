using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] DoorSwitch _switch;
    [SerializeField] float _openSpeed;
    [SerializeField] Transform _openPosition;
    bool _isOpening = false;

    private void OnEnable()
    {
        _switch.OnSwitchActivated += OnSwitchActivated;
    }

    private void OnDisable()
    {
        _switch.OnSwitchActivated -= OnSwitchActivated;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_isOpening)
        {
            transform.position = Vector2.MoveTowards(transform.position, _openPosition.position, _openSpeed * Time.fixedDeltaTime);
            if (transform.position == _openPosition.position) FinishOpenDoor();
        }
    }

    private void FinishOpenDoor()
    {
        Debug.Log("Door opening finish");
        _isOpening = false;
    }

    private void OnSwitchActivated()
    {
        StartOpenDoor();
    }

    private void StartOpenDoor()
    {
        Debug.Log("Door opening start");
        _isOpening = true;
    }
}