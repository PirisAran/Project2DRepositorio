using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivationObject : MonoBehaviour
{
    public Action OnActivated;

    protected virtual void Activate()
    {
        OnActivated?.Invoke();
        DoAnimation();
    }

    protected virtual void DoAnimation()
    {

    }
}
