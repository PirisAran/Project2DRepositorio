using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] DoorSwitch _switch;
    [SerializeField] float _openSpeed;
    [SerializeField] Transform _openPosition;
    [SerializeField] Animator _anim;
    [SerializeField] AnimationClip _activateDoorClip;
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
        StartCoroutine(StartOpenDoor());
    }

    private IEnumerator StartOpenDoor()
    {
        Debug.Log("Door opening start");
        _anim.SetBool("activated", true);
        yield return new WaitForSeconds(_activateDoorClip.length);
        _isOpening = true;
        _anim.SetBool("activated", true);
    }
}
