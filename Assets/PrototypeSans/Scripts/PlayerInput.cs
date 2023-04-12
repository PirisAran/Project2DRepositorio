using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }

    public Action OnJumpStarted;
    public Action OnJumpFinished;
    public Action OnCandleThrown;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            OnJumpStarted?.Invoke();

        if (Input.GetKeyUp(KeyCode.Space))
            OnJumpFinished?.Invoke();
        
        if (Input.GetKeyUp(KeyCode.Q))
            OnCandleThrown?.Invoke();

        MovementHorizontal = Input.GetAxis("Horizontal");
        MovementVertical = Input.GetAxis("Vertical");       
    }
}
