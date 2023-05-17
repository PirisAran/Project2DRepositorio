using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    protected LayerMask _whatIsCollision;
    [SerializeField]
    protected Transform _groundCheckerOrigin;

    public Action OnLanding;

    protected bool _colliding;

    bool _lastColliding = false;
    
    protected abstract bool CheckIfColliding();

    protected void CheckLanding()
    {
        var currentColliding = _colliding;

        if (!_lastColliding && currentColliding)
            OnLanding?.Invoke();

        _lastColliding = currentColliding;
    }

}
