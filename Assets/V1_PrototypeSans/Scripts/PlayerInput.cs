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

    [SerializeField]
    KeyCode CancelThrowKey = KeyCode.Mouse1;

    [SerializeField]
    KeyCode PickUpKey = KeyCode.E;

    public Action OnJumpStarted;
    public Action OnJumpFinished;

    public Action OnThrowStarted;
    public Action OnThrowFinished;

    public Action OnCancelThrowStarted;

    public Action OnTryPickUp;



    // Update is called once per frame
    void Update()
    {
        JumpInput();
        ThrowInput();
        CancelThrowInput();
        MoveInput();
        TryPickUpInput();
    }

    private void TryPickUpInput()
    {
        if (Input.GetKeyDown(PickUpKey))
            OnTryPickUp?.Invoke();
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

    private void CancelThrowInput()
    {
        if (Input.GetKeyDown(CancelThrowKey))
            OnCancelThrowStarted?.Invoke();
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(JumpKey))
            OnJumpStarted?.Invoke();

        if (Input.GetKeyUp(JumpKey))
            OnJumpFinished?.Invoke();
    }
}
