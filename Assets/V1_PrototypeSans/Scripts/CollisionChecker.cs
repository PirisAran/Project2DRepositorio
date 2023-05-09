using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private float DetectionRadius = 0.15f;
    [SerializeField]
    LayerMask WhatIsCollision;
    [SerializeField]
    Transform GroundCheckerOrigin;

    public Action OnLanding;

    public bool Colliding => _colliding;
    bool _colliding;

    bool lastOnGround = false;

    private void Update()
    {
        CheckOnGround();
        CheckLanding();
    }

    private void CheckOnGround()
    {
        _colliding = false;
        var colliders = Physics2D.OverlapCircleAll(GroundCheckerOrigin.position, DetectionRadius, WhatIsCollision);
        _colliding = (colliders.Length > 0);
    }

    private void CheckLanding()
    {
        var currentOnGround = _colliding;

        if (!lastOnGround && currentOnGround)
        {
            OnLanding?.Invoke();
            Debug.Log("landed");
        }

        lastOnGround = currentOnGround;
    }

}
