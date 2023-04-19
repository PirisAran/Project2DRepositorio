using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }

    [SerializeField]
    KeyCode JumpKey = KeyCode.Space;

    [SerializeField]
    KeyCode ThrowKey = KeyCode.Mouse0;

    public Action OnJumpStarted;
    public Action OnJumpFinished;

    public Action OnThrowStarted;
    public Action OnThrowFinished;

    // Update is called once per frame
    void Update()
    {
        JumpInput();
        ThrowInput();
        MoveInput();
    }

    private void MoveInput()
    {
        MovementHorizontal = Input.GetAxis("Horizontal");
        MovementVertical = Input.GetAxis("Vertical");
    }

    private void ThrowInput()
    {
        if (Input.GetKeyDown(ThrowKey))
            OnThrowStarted?.Invoke();

        if (Input.GetKeyUp(ThrowKey))
            OnThrowFinished?.Invoke();
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(JumpKey))
            OnJumpStarted?.Invoke();

        if (Input.GetKeyUp(JumpKey))
            OnJumpFinished?.Invoke();
    }
}
