using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] FireActivationObject _switch;
    [SerializeField] float _openSpeed;
    [SerializeField] Transform _openPosition;
    bool _isOpening = false;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpening)
        {
            Vector2.MoveTowards(transform.position, _openPosition.position, _openSpeed * Time.fixedDeltaTime);
            if (transform.position == _openPosition.position) FinishOpenDoor();
        }
    }

    private void FinishOpenDoor()
    {
        _isOpening = false;
    }

    private void OnSwitchActivated()
    {
        StartOpenDoor();
    }

    private void StartOpenDoor()
    {
        _isOpening = true;
    }
}
